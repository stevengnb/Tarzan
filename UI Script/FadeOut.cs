using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private float durationTransition = 2f;
    [SerializeField] private GameObject panel;
    [SerializeField] private Image image;
    [SerializeField] private AudioSource backgroundSound;
    [SerializeField] private GameObject mainMenu;
    private AsyncLoader asyncLoader;

    public void Fade(string level)
    {
        asyncLoader = FindObjectOfType<AsyncLoader>();
        mainMenu.SetActive(false);
        image = panel.GetComponent<Image>();
        backgroundSound.volume = 0;
        StartCoroutine(DarkTransition(0, 1, durationTransition, level));
    }

    private IEnumerator DarkTransition(float initial, float target, float duration, string level)
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
        yield return StartCoroutine(ChangeScene(1f, level));
    }

    private IEnumerator ChangeScene(float duration, string level)
    {
        yield return new WaitForSeconds(duration);

        asyncLoader.LoadGame(level);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }
}
