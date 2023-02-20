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
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Punch");
        }

        if (Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.D))
        {
            animator.SetTrigger("IsWalking");
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            animator.ResetTrigger("IsWalking");
        }
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
