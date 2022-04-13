using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceUIManager : MonoBehaviour
{
    [SerializeField] private ConsumableResourceUI manaUI;
    [SerializeField] private ConsumableResourceUI staminaUI;

    public ConsumableResourceUI ManaUI  { get => manaUI; }
    public ConsumableResourceUI StaminaUI { get => staminaUI; }
}
