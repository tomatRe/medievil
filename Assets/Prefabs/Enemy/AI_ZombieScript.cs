using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_ZombieScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private new Rigidbody rigidbody;
    private NavMeshAgent ai;
    

    void Awake()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
        if (!rigidbody)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        if (!ai)
        {
            ai = GetComponent<NavMeshAgent>();
        }

    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        
    }
}
