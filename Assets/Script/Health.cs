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
    public UnityEvent OnDie = new UnityEvent();
    Animator animator;
    float Timer = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        animator.SetTrigger("TakeDamage");
        OnTakeDamage.Invoke();
        if (CurrentHealth == 0)
            OnDie.Invoke();
    }

    [ContextMenu("Test Die")]
    void TestDie()
    {
        TakeDamage(MaxHealth);
    }

}
