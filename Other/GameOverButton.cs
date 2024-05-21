using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButton : MonoBehaviour
{
    public void MainMenu()
    {
        Time.timeScale = 1f;
        AudioManagerGame.instance.ButtonClicked();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
