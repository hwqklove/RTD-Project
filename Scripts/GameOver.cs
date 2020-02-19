using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Retry ()
    {
        SceneManager.LoadScene("start");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
