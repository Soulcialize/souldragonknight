using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ToggleableButton : MonoBehaviour, IPointerClickHandler
{
    private bool isToggled = false;
    private Color untoggledColor;
    private Color toggledColor;
    private string untoggledText;
    
    [Header("UI")]

    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private string toggledText;

    [Header("Events")]

    [SerializeField] private UnityEvent toggleOnEvent;
    [SerializeField] private UnityEvent toggleOffEvent;

    void Start()
    {
        
        untoggledColor = button.colors.normalColor;
        toggledColor = button.colors.pressedColor;
        untoggledText = buttonText.text;

        if (toggledText == "")
        {
            toggledText = untoggledText;
        }
    }

    private void UpdateVisuals()
    {
        ColorBlock cb = button.colors;

        if (isToggled)
        {
            cb.normalColor = toggledColor;
            button.colors = cb;
            buttonText.text = toggledText;
        }
        else
        {
            cb.normalColor = untoggledColor;
            button.colors = cb;
            buttonText.text = untoggledText;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isToggled = !isToggled;
        UpdateVisuals();

        if (isToggled)
        {
            toggleOnEvent.Invoke();
        } 
        else
        {
            toggleOffEvent.Invoke();
        }
    }
}
