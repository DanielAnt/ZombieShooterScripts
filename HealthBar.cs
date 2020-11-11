using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Slider sliderObject;
    public TextMeshProUGUI healthBarText;

    void Start()
    {
        sliderObject = GetComponentInChildren<Slider>();
        healthBarText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetHealth(float hitPoints)
    {
        healthBarText.text = $"{hitPoints}/100";
        sliderObject.value = hitPoints / 100;
    }


}
