using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //[SerializeField] private float cameraSensitivity = 2;
    [SerializeField] private float speed = 1;
    [SerializeField] private float jumpSpeed = 10;
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

        Vector3 cameraVector = camera.transform.forward;

        GetComponent<Rigidbody>().velocity = 
            new Vector3(
                vertical * (speed * cameraVector.x) + horizontal * (speed * cameraVector.z),
                0,
                vertical * (speed * cameraVector.z) + horizontal * (speed * cameraVector.x));

        if (jump > 0)
        {
            Vector3 fuerzaSalto = new Vector3(0, jumpSpeed, 0);
            GetComponent<Rigidbody>().AddForce(fuerzaSalto);
        }

        AnimateMovement(horizontal, vertical);
    }

    void AnimateMovement(float horizontal, float vertical)
    {
        if (horizontal > 0.1f || horizontal < -0.1f || vertical > 0.1f || vertical < -0.1f)
        {
            anim.Play("run");

            if (horizontal > 0.1f)//Derecha
            {
                model.rotation = new Quaternion(0,1,0,1);
            }
            if (horizontal < -0.1f)//Izquierda
            {
                model.rotation = new Quaternion(0, -1, 0, 1);
            }
            if (vertical > 0.1f)//Arriba
            {
                model.rotation = new Quaternion(0, 1, 0, 0);
            }
            if (vertical < -0.1f)//Abajo
            {
                model.rotation = new Quaternion(0, -1, 0, 0);
            }

        }
        else
        {
            anim.Play("idle");
        }
    }
}
