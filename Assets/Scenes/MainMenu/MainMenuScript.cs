using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneManager.LoadScene("level1");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
