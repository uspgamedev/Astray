using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void Restart()
    {
        FindObjectOfType<AudioManager>().Play("MainTheme");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        FindObjectOfType<AudioManager>().Play("MainTheme");
        SceneManager.LoadScene("Menu");
    }
}
