using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EscapeMenu : MonoBehaviour
{
    public static EscapeMenu Instance { get; private set; }

    [SerializeField] private GameObject escapeMenuUiObject;
    [SerializeField] private UnityEvent escapeMenuOpenEvent;
    [SerializeField] private UnityEvent escapeMenuCloseEvent;

    public bool IsMenuOpen { get; private set; }
    public UnityEvent EscapeMenuOpenEvent { get => escapeMenuOpenEvent; }
    public UnityEvent EscapeMenuCloseEvent { get => escapeMenuCloseEvent; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ToggleMenu()
    {
        if (IsMenuOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }     
    }

    public void OpenMenu()
    {
        escapeMenuUiObject.SetActive(true);
        IsMenuOpen = true;
        escapeMenuOpenEvent.Invoke();
    }

    public void CloseMenu()
    {
        escapeMenuUiObject.SetActive(false);
        IsMenuOpen = false;
        escapeMenuCloseEvent.Invoke();
    }
}
