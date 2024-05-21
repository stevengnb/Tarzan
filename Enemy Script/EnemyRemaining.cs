using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyRemaining : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI remainingText;

    public void SetHealth(int curr, int max)
    {
        slider.value = curr;
        remainingText.SetText(curr.ToString() + "/" + max.ToString() + " remaining");
    }

    public void SetMaxHealth(int curr, int max)
    {
        slider.maxValue = max;
        slider.value = curr;
        remainingText.SetText(curr.ToString() + "/" + max.ToString() + " remaining");
    }
}
