using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_ZombieScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private float pursuitDuration;
    private NavMeshAgent ai;

    //Movement
    public float speed = 5;
    public float directionChangeInterval = 1;
    public float maxHeadingChange = 30;
    float heading;
    Vector3 targetRotation;

    //IA States
    private int currentState = 0;
    private string[] states = { "wandering", "pursuit", "attacking"};
    private float CurrentPursuitDuration = 0;
    Transform playerLocation;

    //Attack variables
    [SerializeField]private float attackDuration = 1;
    private float currentAttackDuration = 0;


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

    private void Start()
    {
        heading = UnityEngine.Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());
    }

    private void Update()
    {
        Debug.Log(states[currentState]);
        switch (currentState)
        {
            case 0:
                Wander();
                break;

            case 1:
                MoveToPlayer();
                break;

            case 2:
                Attack();
                break;

            default:
                Wander();
                break;
        }
    }

    void Attack()
    {
        currentAttackDuration += Time.deltaTime;
        currentState = 2;
        animator.SetBool("Attack", true);

        if (currentAttackDuration >= attackDuration)
        {
            currentState = 1;
            animator.SetBool("Attack", false);
        }
    }

    void Wander()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);

        GetComponent<Rigidbody>().velocity = forward * speed * Time.deltaTime;
        animator.SetFloat("MoveSpeed", (forward * speed * Time.deltaTime).magnitude);
    }

    void MoveToPlayer()
    {
        CurrentPursuitDuration += Time.deltaTime;
        ai.SetDestination(playerLocation.position);

        Debug.Log(CurrentPursuitDuration);

        if (CurrentPursuitDuration >= pursuitDuration)
        {
            currentState = 0;
            CurrentPursuitDuration = 0;
        }
    }

    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }


    void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = UnityEngine.Random.Range(floor, ceil);
        targetRotation = new Vector3(0, heading, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerLocation = other.transform;
            currentState = 1;
            //MoveToPlayer();
        }
    }
}
