﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private float health = 100;
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpSpeed = 100;
    [SerializeField] private float AttackDuration = 1;
    [SerializeField] private int score = 0;
    [SerializeField] private bool hasSilverKey = false;
    [SerializeField] private bool hasGoldKey = false;
    [SerializeField] private Transform model;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform weaponCollision;

    //UI
    [SerializeField] private Text canvasScoreText;
    [SerializeField] private Text canvasText;
    [SerializeField] private Image canvasGoldKey;
    [SerializeField] private Image canvasSilverKey;
    [SerializeField] private Sprite missingGoldKeySprite;
    [SerializeField] private Sprite missingSilverKeySprite;
    [SerializeField] private Sprite goldKeySprite;
    [SerializeField] private Sprite silverKeySprite;



    //Audio
    private AudioSource source;
    [SerializeField] private AudioClip[] steps;
    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip pickUpKey;
    [SerializeField] private AudioClip pickUpGold;

    private Animation anim;
    private float attackTime = 0;
    private bool attacking = false;


    void Start()
    {
        anim = model.GetComponent<Animation>();
        source = GetComponent<AudioSource>();

        canvasScoreText.text = "Score " + score;
        canvasText.text = "Health " + health;
    }

    void Update()
    {
        Move();
        if (Input.GetButton("Fire1"))
        {
            Attack();
        }
    }

    void Attack()
    {
        anim.Play("attack");
        weaponCollision.GetComponent<BoxCollider>().enabled = true;
        attacking = true;

        source.clip = attack;
        source.Play();
    }

    void TakeDamage()
    {
        health -= 15;

        source.clip = damage;
        source.Play();

        canvasText.text = "Health " + health;

        if (IsGrounded())
        {
            Vector3 fuerzaSalto = new Vector3
                (GetComponent<Rigidbody>().velocity.x,
                50, GetComponent<Rigidbody>().velocity.z);

            GetComponent<Rigidbody>().AddForce(fuerzaSalto, ForceMode.Impulse);
        }

        if (health <= 0)
        {
            UnityEngine.SceneManagement.
                SceneManager.LoadScene("GameOverScreen");
        }
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float jump = Input.GetAxis("Jump");

        Vector3 speedVector = new Vector3
            (horizontal * speed * Time.deltaTime,
            GetComponent<Rigidbody>().velocity.y,
            vertical * speed * Time.deltaTime);

        GetComponent<Rigidbody>().velocity = speedVector;

        AnimateMovement(horizontal, vertical);
    }

    private void AnimateMovement(float horizontal, float vertical)
    {
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (!attacking)
        {
            if (horizontal > 0.1f || horizontal < -0.1f ||
                vertical > 0.1f || vertical < -0.1f)
            {
                anim.Play("run");
                if (!source.isPlaying)
                {
                    source.clip = steps[Random.Range(0, steps.Length)];
                    source.Play();
                }

                if (camera.transform.rotation.y > 90 ||
                    camera.transform.rotation.y < -90)
                {
                    transform.rotation = Quaternion.Slerp
                        (transform.rotation,
                        Quaternion.LookRotation(-movement), 0.15F);
                }
                else
                {
                    transform.rotation = Quaternion.Slerp
                        (transform.rotation,
                        Quaternion.LookRotation(movement), 0.15F);
                }


            }
            else
            {
                anim.Play("idle");
            }
        }
        else
        {
            //Debug.Log(attackTime);
            attackTime += Time.deltaTime;
            if (attackTime >= AttackDuration)
            {
                attacking = false;
                attackTime = 0;
                weaponCollision.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast
            (transform.position, -Vector3.up,
            GetComponent<CapsuleCollider>().bounds.extents.y - 0.25f);
    }

    private void PickUp()
    {
        score += 7;

        if (health + 15 < 250)
            health += 15;

        source.clip = pickUpGold;
        source.Play();

        canvasScoreText.text = "Score " + score;
        canvasText.text = "Health " + health;
    }

    private void PickUpKey()
    {
        source.clip = pickUpKey;
        source.Play();
    }

    private void OpenGate()
    {
        source.clip = openDoor;
        source.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (Vector3.Distance(other.transform.position, transform.position) <= 3)
            {
                TakeDamage();
                other.SendMessage("Attack");
            }
            
        }

        if (other.tag == "SilverKey" && !hasSilverKey)
        {
            hasSilverKey = true;
            Destroy(other.gameObject);
            PickUpKey();
            canvasSilverKey.sprite = silverKeySprite;
        }

        if (other.tag == "GoldKey" && !hasGoldKey)
        {
            hasGoldKey = true;
            Destroy(other.gameObject);
            PickUpKey();
            canvasGoldKey.sprite = goldKeySprite;
        }

        if (other.tag == "GoldenGate" && hasGoldKey)
        {
            hasGoldKey = false;
            other.GetComponent<Animator>().enabled = true;
            OpenGate();
            canvasGoldKey.sprite = missingGoldKeySprite;
        }

        if (other.tag == "SilverGate" && hasSilverKey)
        {
            hasSilverKey = false;
            other.GetComponent<Animator>().enabled = true;
            OpenGate();
            canvasSilverKey.sprite = missingSilverKeySprite;
        }

        if (other.tag == "Finish")
        {
            UnityEngine.SceneManagement.
                SceneManager.LoadScene(0);
        }
    }
}
