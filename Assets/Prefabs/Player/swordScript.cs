using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordScript : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" &&
            Vector3.Distance(other.transform.position,
            transform.position) <= 3.5f)
        {
            other.SendMessage("TakeDamage");
        }
    }
}
