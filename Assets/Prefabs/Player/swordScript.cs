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
        if (other.tag != "Player")
        {
            other.SendMessage("TakeDamage");
        }
    }
}
