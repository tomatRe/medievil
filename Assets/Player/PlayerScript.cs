using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //[SerializeField] private float cameraSensitivity = 2;
    [SerializeField] private float speed = 1;
    [SerializeField] private float jumpSpeed = 10;
    [SerializeField] private Transform model;
    private Animation anim;
    private new Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        
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
        transform.Translate(vertical * (speed * cameraVector.x), 0, vertical * (speed * cameraVector.z));
        transform.Translate(horizontal * (speed * cameraVector.z), 0, horizontal * (speed * cameraVector.x));

        model.Rotate(horizontal, 0, vertical);

        if (jump > 0)
        {
            Vector3 fuerzaSalto = new Vector3(0, jumpSpeed, 0);
            GetComponent<Rigidbody>().AddForce(fuerzaSalto);
        }

        Animate(horizontal, vertical);
    }

    void Animate(float horizontal, float vertical)
    {
        if (horizontal > 0.1f || horizontal < -0.1f || vertical > 0.1f || vertical < -0.1f)
        {
            anim.Play("run");
        }
        else
        {
            anim.Play("idle");
        }
    }
}
