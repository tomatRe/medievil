using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_ZombieScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private new Rigidbody rigidbody;
    private NavMeshAgent ai;
    public float speed = 5;
    public float directionChangeInterval = 1;
    public float maxHeadingChange = 30;
    float heading;
    Vector3 targetRotation;


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
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());
    }

    private void Update()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);

        GetComponent<Rigidbody>().velocity = forward*speed*Time.deltaTime;


        if (GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
        {
            
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
        heading = Random.Range(floor, ceil);
        targetRotation = new Vector3(0, heading, 0);
    }

    void Move()
    {
        //ai.SetDestination();
    }
}
