using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ToGamePlay()
    {
        SceneManager.LoadScene("InGameLevel");
    }

    public void ToExit()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
