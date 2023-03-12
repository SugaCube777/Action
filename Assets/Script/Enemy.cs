using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //Enemy

    public float AttackRange = 1;
    public float AttackDamage = 10;

    public Transform HandPoint;
    public List<Transform> Waypoints = new List<Transform>();
    int WaypointIndex = 0;
    public Transform ScanA;
    public Transform ScanB;

    public State CurrentState = State.idle;

    float FSMTimer = 0;
    Transform target;

    List<Health> attackedCharacters = new List<Health>();
    Coroutine iAttack;
    Coroutine iDie;

    Animator animator;
    NavMeshAgent navMeshAgent;
    Health health;
    float myhealth;


    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        myhealth = health.CurrentHealth;
    }

    private void Update()
    {
        FSM();

        /*
        if (myhealth > 0)
        {
            FSM();
        }
        else if (myhealth <=0)
        { 
            Death();
            iDie = StartCoroutine(IDie());
            Destroy(gameObject);
        }
        */
        
    }

    public void Death()
    {
        animator.SetBool("Death", true);
    }

    void FSM()
    {
        switch (CurrentState)
        {

            case State.idle:
                FSMTimer += Time.deltaTime;
                if (FSMTimer >= 1)
                {
                    FSMTimer = 0;
                    ToPatrol();
                }
                if (target != null)
                    ToChase();
                break;

            case State.patrol:
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    ToIdle();
                if (target != null)
                    ToChase();
                break;

            case State.chase:
                navMeshAgent.SetDestination(target.position);
                if (Vector3.Distance(transform.position, target.position) <= AttackRange)
                    ToAttack();
                break;

            case State.attack:
                FSMTimer += Time.deltaTime;
                if (FSMTimer >= 2)
                {
                    FSMTimer = 0;
                    ToChase();
                }
                break;

        }
        CheckPlayer();
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }


    void CheckPlayer()
    {
        if (target != null) return;
        var Scan = Physics.OverlapCapsule(ScanA.position, ScanB.position, 3);
        foreach (var collider in Scan)
        {
            if (collider.CompareTag("Player"))
            {
                target = collider.transform;
            }
        }
        

        
    }

    void ToIdle()
    {
        CurrentState = State.idle;
    }

    void ToPatrol()
    {
        CurrentState = State.patrol;
        WaypointIndex = (WaypointIndex + 1) % Waypoints.Count;
        navMeshAgent.SetDestination(Waypoints[WaypointIndex].position);
    }

    void ToChase()
    {
        CurrentState = State.chase;
    }

    void ToAttack()
    {
        CurrentState = State.attack;
        animator.SetTrigger("Attack");
    }

    public void ToDie()
    {
        CurrentState = State.die;
        animator.SetBool("Death", true);
        StartCoroutine(IDie());
    }

  
    public void Attack()
    {
        Collider[] Scan = Physics.OverlapSphere(HandPoint.position, 1);
        foreach (Collider collider in Scan)
        {
            Health health = collider.GetComponent<Health>();
            if (health != null && collider.CompareTag("Player"))
            {
                health.TakeDamage(AttackDamage);
            }
        }
    }

    public void AttackStart()
    {
        iAttack = StartCoroutine(IAttack());
    }

    public void AttackEnd()
    {
        attackedCharacters.Clear();
        StopCoroutine(iAttack);
    }

    IEnumerator IAttack()
    {
        List<Health> attacked = new List<Health>();
        while (true)
        {
            Collider[] scan = Physics.OverlapSphere(HandPoint.position, 1);
            foreach (Collider collider in scan)
            {
                Health health = collider.GetComponent<Health>();
                if (health != null && collider.CompareTag("Player"))
                {
                    health.TakeDamage(AttackDamage);
                    attackedCharacters.Add(health);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    
    IEnumerator IDie()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    
    public enum State { idle, patrol, chase, attack, die };

}
