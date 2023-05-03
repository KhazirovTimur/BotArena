using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SimpleEnemy : MonoBehaviour, IDamagable
{

    public int HP;

    public void TakeDamage(float damage)
    {
        Debug.Log("You hit me on " + damage + " damage");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
