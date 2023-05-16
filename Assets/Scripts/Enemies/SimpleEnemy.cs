using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SimpleEnemy : MonoBehaviour, IDamagable
{
    [SerializeField]
    private float hp;
    [SerializeField]
    private float valueInMoney;

    public void TakeDamage(float damage)
    {
        Debug.Log("You hit me on " + damage + " damage");
        hp -= damage;
        CheckHP();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckHP()
    {
        if (hp <= 0)
            KillThisEnemy();        
    }

    private void KillThisEnemy()
    {
        Debug.Log("I'm dead(");
        Destroy(this.gameObject);
    }
}
