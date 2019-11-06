using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject zombie;
    [SerializeField] private int ammountOfZombies = 3;
    [SerializeField] private int spawnDelay = 0;
    [SerializeField] private int spawnRadius = 3;
    private bool done = false;
    private float timeElapsed = 0;

    void Start()
    {
        if (zombie == null)
        {
            Debug.LogError("Zombie spawner dont have zombie assigned");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!done)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= spawnDelay)
            {
                done = true;
                Spawn();
            }
        }
    }

    void Spawn()
    {
        for (int i = 0; i < ammountOfZombies; i++)
        {
            Instantiate(zombie, transform);
        }
    }
}
