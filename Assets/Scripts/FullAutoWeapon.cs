using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullAutoWeapon : MonoBehaviour, IWeapon
{
    public GameObject bullet;
    
    private bool _triggerIsPushed;
    private bool _triggerWasReleased;
    private int _capacity;
    private float _delay;

    private Vector3 _target;

    public WeaponDataSO weaponData;
    
    

    // Start is called before the first frame update
    void Start()
    {
        _triggerIsPushed = false;
        _triggerWasReleased = true;
        _capacity = weaponData.Capacity;
    }

    // Update is called once per frame
    void Update()
    {

        if (_triggerIsPushed && Time.time > _delay && _capacity > 0)
        {
            Shoot();
        }

        if (_capacity == 0)
        {
            Reload();
        }
        
    }


    public void Shoot()
    {
        _triggerWasReleased = false;
        _capacity -= 1;
        GameObject newBullet = Instantiate(bullet, transform);
        newBullet.transform.LookAt(_target);
        newBullet.transform.parent = null;
        _delay = Time.time + weaponData.ShotDelay;
    }

    public void Reload()
    {
        StartCoroutine(Reloading());
    }

    public void TriggerPushed(bool triggerStatePushed, Vector3 pointOnTarget)
    {
        _triggerIsPushed = triggerStatePushed;
        _target = pointOnTarget;
    }

    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(3);
        _capacity = weaponData.Capacity;
    }
}
