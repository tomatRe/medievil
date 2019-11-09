using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_ZombieScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject coins;
    [SerializeField] private Transform coinLocation;
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private float health = 30;
    [SerializeField] private int deSpawnDelay = 40;
    private NavMeshAgent ai;
    private bool dead = false;
    private float currentLifeSpan = 0;

    //Sound
    private AudioSource source;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip growling;

    //Movement
    public float speed = 5;
    public float directionChangeInterval = 1;
    public float maxHeadingChange = 30;
    float heading;
    Vector3 targetRotation;

    //IA States
    [SerializeField] private float pursuitDuration;
    private int currentState = 0;
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
        source = GetComponent<AudioSource>();
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(NewHeading());
    }

    private void Update()
    {
        if (!dead)
        {
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
        else
        {
            currentLifeSpan += Time.deltaTime;

            if (currentLifeSpan >= deSpawnDelay)
            {
                Destroy(gameObject);
            }
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

    void TakeDamage()
    {
        health -= 15;
        source.clip = hit;
        source.Play();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        dead = true;
        maxHeadingChange = 0;

        animator.SetBool("Attack", false);
        animator.SetBool("Dead", true);

        GetComponent<NavMeshAgent>().speed = 0;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;

        SpawnLoot();
        tag = "Dead";
    }

    void SpawnLoot()
    {
        if (coins != null)
            Instantiate(coins, coinLocation, false);
    }

    void Wander()
    {
        if (!dead)
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
            var forward = transform.TransformDirection(Vector3.forward);

            GetComponent<Rigidbody>().velocity = forward * speed * Time.deltaTime;
            animator.SetFloat("MoveSpeed", (forward * speed * Time.deltaTime).magnitude);

            if (Random.Range(0, 1000) >= 990)//provability of growling
            {
                if (!source.isPlaying)
                {
                    //source.clip = growling;
                    //source.Play();
                }
            }
        }
    }

    void MoveToPlayer()
    {
        CurrentPursuitDuration += Time.deltaTime;
        ai.SetDestination(playerLocation.position);
        animator.SetFloat("MoveSpeed", 300);

        //Debug.Log(CurrentPursuitDuration);

        if (CurrentPursuitDuration >= pursuitDuration && !dead)
        {
            currentState = 0;
            CurrentPursuitDuration = 0;
        }
    }

    IEnumerator NewHeading()
    {
        while (!dead)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }


    void NewHeadingRoutine()
    {
        if (!dead)
        {
            var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
            var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
            heading = Random.Range(floor, ceil);
            targetRotation = new Vector3(0, heading, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !dead)
        {
            playerLocation = other.transform;
            currentState = 1;
            //MoveToPlayer();
        }
    }
}
