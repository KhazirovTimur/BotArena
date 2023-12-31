﻿using System;
using System.Collections;
using Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Move")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 12.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;
		
		[Space(10)]
		[Header("Dash")]
		[Tooltip("Dash initial speed")]
		public float DashStartSpeed = 26.0f;
		[Tooltip("Time required to pass before being able to dash again. Set to 0f to instantly dash again")]
		public float DashTimeout = 1.5f;
		[Tooltip("How long dash works")]
		public float DashTime = 0.001f;
		[Tooltip("If the character is in dash or not")]
		[HideInInspector]
		public bool InDash = false;
		
		[Space(10)]
		[Header("Jump")]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		[Space(10)]
		[Header("Weapons")]
		public float WeaponShakeCoef = 0.28f;
		public LayerMask targetRaycastOcclusionLayers;

		[Space(10)] 
		[Header("Interactions")] 
		public LayerMask interactionRaycastLayers;
		public float interactDistance;
		
		//Actions
		public Action<IInteractable> LookAtInteractable;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;
		
		//dash
		private Vector3 DashCurrentDirection;
		private float _dashTimeoutDelta;
		private float _dashTimeDelta;
		
		

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;
		
		//Link to player inventory
		private PlayerInventory _playerInventory;
		private PlayerInterface _playerInterface;

		private RaycastHit hit;

	
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;
		private CinemachineVirtualCamera _weaponFollowCamera;
		private CinemachineBasicMultiChannelPerlin _weaponCameraNoise;

		private const float _threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			_weaponFollowCamera = GameObject.FindGameObjectWithTag("WeaponFollowCamera")
				.GetComponent<CinemachineVirtualCamera>();
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
			_playerInventory = GetComponentInChildren<PlayerInventory>();
			_playerInterface = GetComponent<PlayerInterface>();
			_input.ChooseWeapon += SetActiveWeapon;
		}


		
		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();
			Dash();
			TriggerPushed();
			WeaponShakeAnimation();
			Interaction();
			ShootGrenade();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			if(InDash)  //disable move controller while dashing
				return;
			// set target speed based on move speed
			float targetSpeed = MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}


		public void LaunchInAir(Vector3 dirrection, float startSpeed)
		{
			dirrection = dirrection.normalized;
			Vector3 projectOnGround = Vector3.ProjectOnPlane(dirrection, Vector3.up);
			_speed = projectOnGround.magnitude * startSpeed;
			_verticalVelocity = (dirrection - projectOnGround).magnitude * startSpeed;
		}
		


		private void WeaponShakeAnimation()
		{
			if (!Grounded || InDash)
			{
				_weaponFollowCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
				return;
			}

			_weaponFollowCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 
				(1 + _controller.velocity.magnitude) * WeaponShakeCoef;
		}

		

		private void TriggerPushed()
		{
			//Always send information: "Is player pushing fire button or not?"; "Where is he aiming?"
			if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, Mathf.Infinity, targetRaycastOcclusionLayers))
			{
				_playerInventory.TriggerPushed(_input.triggerPushed, _mainCamera.transform.position + (_mainCamera.transform.forward * hit.distance));
			}
			else
			{
				_playerInventory.TriggerPushed(_input.triggerPushed, _mainCamera.transform.position + _mainCamera.transform.forward * 1000);
			}
		}


		private void Dash()
		{
			if (!InDash)
			{
				//initiate dash
				if (_input.dash && !InDash && _dashTimeoutDelta <= 0 && _input.move != Vector2.zero)
				{
					InDash = true;
					_dashTimeDelta = DashTime;
					DashCurrentDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
					DashCurrentDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
					_speed = DashStartSpeed;
				}
				//dash cooldown
				if (_dashTimeoutDelta >= 0.0f)
				{
					_dashTimeoutDelta -= Time.deltaTime;
				}
				
			}
			else
			{
				_input.dash = false;
				//Moving controller in dash, reducing dash timer
				if (_dashTimeDelta >= 0.0f)
				{
					_dashTimeDelta -= Time.deltaTime;
					_controller.Move(DashCurrentDirection.normalized * (_speed * Time.deltaTime));	
					float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
					_speed = Mathf.Lerp(currentHorizontalSpeed, MoveSpeed,  ( DashTime - _dashTimeDelta)/ DashTime);
				}
				else
				{  //stopping dash, initiating cooldown timer
					InDash = false;
					_input.dash = false;
					_dashTimeoutDelta = DashTimeout;
				}

			}

		}

		private void Interaction()
		{
			if(Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out RaycastHit hit, interactDistance,interactionRaycastLayers))
			{
				if (hit.transform.TryGetComponent(out IInteractable interactItem))
				{
					LookAtInteractable?.Invoke(interactItem);
					if (!_input.interact)
						return;
					interactItem.OnInteraction(_playerInterface);
					_input.interact = false;
				}
			}
			_input.interact = false;
		}

		private void ShootGrenade()
		{
			_playerInventory.ShootGrenade(_input.shootGrenade);
		}

		private void SetActiveWeapon(int weaponIndex)
		{
			_playerInventory.StartWeaponChange(weaponIndex);
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
		
	}
}