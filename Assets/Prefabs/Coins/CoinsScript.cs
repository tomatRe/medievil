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
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && currentTime >= spawnDelay)
        {
            other.SendMessage("PickUp");
            Destroy(gameObject);
        }
    }
}
