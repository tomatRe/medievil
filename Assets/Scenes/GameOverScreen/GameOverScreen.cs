using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    float seconds = 0;
    void Update()
    {
        seconds += Time.deltaTime;

        if (seconds >= 5)
        {
            SceneManager.LoadScene(0);
        }
    }
}
