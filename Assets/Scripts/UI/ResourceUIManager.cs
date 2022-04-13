using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUIManager : MonoBehaviour
{
    private static ResourceUIManager _instance;

    public ConsumableResourceUI ManaUI  { get => manaUI; }
    public ConsumableResourceUI StaminaUI { get => staminaUI; }
    public ResourceUIManager Instance { get => _instance; }

    [SerializeField] private ConsumableResourceUI manaUI;
    [SerializeField] private ConsumableResourceUI staminaUI;

    private void Awake()
    {
        // singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
