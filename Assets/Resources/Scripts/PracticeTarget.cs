using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeTarget : MonoBehaviour, IDamagable
{
   public void TakeDamage(float damage)
   {
      Debug.Log($"I took {damage} damage");
   }
}
