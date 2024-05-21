using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TowerHealth : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image inside;

    public void SetHealth(int curr)
    {
        slider.value = curr;
    }

    public void SetMaxHealth(int curr, int max)
    {
        slider.maxValue = max;
        slider.value = curr;
    }
}
