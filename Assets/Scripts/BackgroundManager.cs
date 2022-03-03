using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance { get; private set; }

    [SerializeField] private GameObject realWorldBackground;
    [SerializeField] private GameObject soulWorldBackground;

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

    public void ActivateRealWorldBackground()
    {
        soulWorldBackground.SetActive(false);
        realWorldBackground.SetActive(true);
    }

    public void ActivateSoulWorldBackground()
    {
        realWorldBackground.SetActive(false);
        soulWorldBackground.SetActive(true);
    }
}
