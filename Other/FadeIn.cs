using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private float durationTransition = 2f;
    [SerializeField] private GameObject panel;
    private Image image;

    private void Start()
    {
        image = panel.GetComponent<Image>();
        StartFadeIn();
    }

    public void StartFadeIn()
    {
        StartCoroutine(LightTransition(1, 0, durationTransition));
    }

    private IEnumerator LightTransition(float initial, float target, float duration)
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
    }
}
