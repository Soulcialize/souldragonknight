using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleableButton : MonoBehaviour, IPointerClickHandler
{
    private bool isToggled = false;
    private Color untoggledColor;
    private Color toggledColor;
    
    [SerializeField] private Button button;
    [SerializeField] private UnityEvent toggleOnEvent;
    [SerializeField] private UnityEvent toggleOffEvent;

    public UnityEvent ToggleOnEvent { get => toggleOnEvent; }
    public UnityEvent ToggleOffEvent { get => toggleOffEvent; }

    void Start()
    {
        untoggledColor = button.colors.normalColor;
        toggledColor = button.colors.pressedColor;
    }

    private void UpdateColor()
    {
        ColorBlock cb = button.colors;

        if (isToggled)
        {
            cb.normalColor = toggledColor;
            button.colors = cb;
        }
        else
        {
            cb.normalColor = untoggledColor;
            button.colors = cb;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isToggled = !isToggled;
        UpdateColor();

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
