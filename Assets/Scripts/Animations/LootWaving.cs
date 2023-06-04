using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootWaving : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.1f;

    private float _frequencyDelta;
    private float _phaseDelta;

    private void OnEnable()
    {
        _frequencyDelta = Random.Range(1.0f, 2.5f);
        _phaseDelta = Random.Range(0.0f, 1.0f);
    }


    private void Update()
    {
        transform.position += new Vector3(0, (float)(Math.Cos(_frequencyDelta * (Time.time + _phaseDelta)) * Time.deltaTime), 0) * amplitude;
    }
}
