using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Player
    public float AttackDamage = 10;

    public Transform HandPoint;

    Animator animator;

    void Start()
    {

    }
    void Update()
    {

    }

    public void Attack()
    {
        Collider[] scan = Physics.OverlapSphere(HandPoint.position, 1);
        foreach (Collider collider in scan)
        {
            Health health = collider.GetComponent<Health>();
            if (health != null && collider.CompareTag("Enemy"))
            {
                health.TakeDamage(AttackDamage);
            }
        }
    }

}
