using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    public void QuitButton()
    {
        Application.Quit();
    }

    public void ReTry()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
