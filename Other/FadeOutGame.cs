using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutGame : MonoBehaviour
{
    private float durationTransition = 2f;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelVictory;
    private Image image;
    public int status;

    public void StartFadeOut()
    {
        panel.SetActive(true);
        image = panel.GetComponent<Image>();
        AudioManagerGame.instance.bgmAudio.volume = 0.1f;
        StartCoroutine(DarkTransition(0, 1, durationTransition));
    }

    private IEnumerator DarkTransition(float initial, float target, float duration)
    {
        Color initialColor = image.color;
        Color targetColor = image.color;
        initialColor.a = initial;
        targetColor.a = target;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            image.color = Color.Lerp(initialColor, targetColor, t);

            yield return null;
        }

        image.color = targetColor;
        panel.SetActive(false);
        if(status == 1)
        {
            panelGameOver.SetActive(true);
        } else if(status == 2)
        {
            panelVictory.SetActive(true);
        }
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.instance.isEndGame = false;
    }
}
