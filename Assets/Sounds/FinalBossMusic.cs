using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossMusic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
