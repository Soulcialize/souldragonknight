using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance { get; private set; }

    [SerializeField] private GameObject realWorldBackgrounds;
    [SerializeField] private GameObject soulWorldBackgrounds;

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
        soulWorldBackgrounds.SetActive(false);
        realWorldBackgrounds.SetActive(true);
    }

    public void ActivateSoulWorldBackground()
    {
        realWorldBackgrounds.SetActive(false);
        soulWorldBackgrounds.SetActive(true);
    }
}
