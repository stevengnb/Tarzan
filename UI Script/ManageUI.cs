using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ManageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI petsText;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        petsText.SetText("You have " + PlayerStats.instance.TotalPet().ToString() + " pets.");
        levelText.SetText(PlayerStats.instance.Level.ToString());
    }

    private void Update()
    {
        petsText.SetText("You have " + PlayerStats.instance.TotalPet().ToString() + " pets.");
        levelText.SetText(PlayerStats.instance.Level.ToString());
    }
}
