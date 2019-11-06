using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsScript : MonoBehaviour
{
    float currentTime = 0;
    int spawnDelay = 1;
    private void Start()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= spawnDelay)
        {
            GetComponent<CapsuleCollider>().enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.SendMessage("PickUp");
            Destroy(gameObject);
        }
    }
}
