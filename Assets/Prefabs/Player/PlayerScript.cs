﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private float health = 100;
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpSpeed = 100;
    [SerializeField] private float AttackDuration = 1;
    [SerializeField] private Transform model;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform weaponCollision;
    private Animation anim;
    private float attackTime = 0;
    bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = model.GetComponent<Animation>();
    }

    // Update is called once per frame
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
    }

    void TakeDamage()
    {
        health -= 15;


        Vector3 fuerzaSalto = new Vector3
                (GetComponent<Rigidbody>().velocity.x,
                30, GetComponent<Rigidbody>().velocity.z);

        GetComponent<Rigidbody>().AddForce(fuerzaSalto);
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

        if (jump > 0)
        {
            Vector3 fuerzaSalto = new Vector3
                (GetComponent<Rigidbody>().velocity.x,
                jumpSpeed, GetComponent<Rigidbody>().velocity.z);

            GetComponent<Rigidbody>().AddForce(fuerzaSalto);
        }

        AnimateMovement(horizontal, vertical);
    }

    void AnimateMovement(float horizontal, float vertical)
    {
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (!attacking)
        {
            if (horizontal > 0.1f || horizontal < -0.1f ||
                vertical > 0.1f || vertical < -0.1f)
            {
                anim.Play("run");

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
    }
}
