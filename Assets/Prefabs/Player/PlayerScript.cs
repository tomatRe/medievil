using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //[SerializeField] private float cameraSensitivity = 2;
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpSpeed = 100;
    [SerializeField] private Transform model;
    [SerializeField] private new Camera camera;
    private Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = model.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float jump = Input.GetAxis("Jump");

        /*
         * Movement relative to the camera
         * 
        Vector3 moveVector = new Vector3(transform.position.x * horizontal,
            0f, (transform.position.z - camera.transform.position.z) * vertical);

        GetComponent<Rigidbody>().AddForce(moveVector * speed * Time.deltaTime, ForceMode.Acceleration);

        Debug.Log(moveVector);

        */

        GetComponent<Rigidbody>().velocity = (new Vector3(horizontal * speed * Time.deltaTime, GetComponent<Rigidbody>().velocity.y, vertical * speed * Time.deltaTime));

        if (jump > 0)
        {
            Vector3 fuerzaSalto = new Vector3(GetComponent<Rigidbody>().velocity.x, jumpSpeed, GetComponent<Rigidbody>().velocity.z);
            GetComponent<Rigidbody>().AddForce(fuerzaSalto);
        }

        AnimateMovement(horizontal, vertical);
    }

    void AnimateMovement(float horizontal, float vertical)
    {
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (horizontal > 0.1f || horizontal < -0.1f || vertical > 0.1f || vertical < -0.1f)
        {
            anim.Play("run");

            if (camera.transform.rotation.y > 90 || camera.transform.rotation.y < -90)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-movement), 0.15F);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
            }

            
        }
        else
        {
            anim.Play("idle");
        }
    }
}
