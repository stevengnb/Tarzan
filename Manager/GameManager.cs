using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public bool isEndGame = false;
    private FadeOutGame fadeOut;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndGame(int index)
    {
        if (!isEndGame)
        {
            isEndGame = true;
            fadeOut = FindObjectOfType<FadeOutGame>();
            fadeOut.status = index;

            fadeOut.StartFadeOut();
            AudioManagerGame.instance.GameOverSound();
        }
    }
}
