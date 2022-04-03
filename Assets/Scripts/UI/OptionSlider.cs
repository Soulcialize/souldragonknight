using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class OptionSlider : MonoBehaviour
{
    [SerializeField] protected Slider slider;
    [SerializeField] protected TextMeshProUGUI valueText;

    protected void OnEnable()
    {
        slider.onValueChanged.AddListener(UpdateValue);
    }
    protected void OnDisable()
    {
        slider.onValueChanged.RemoveListener(UpdateValue);
    }

    protected virtual void UpdateValue(float value)
    {
        valueText.text = value.ToString();
    }
}
