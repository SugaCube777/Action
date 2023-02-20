using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    //Health script

    public float CurrentHealth = 100;
    public float MaxHealth = 100;
    public UnityEvent OnTakeDamage = new UnityEvent();
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        OnTakeDamage.Invoke();
    }

}
