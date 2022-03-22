using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HealthUpdateEvent : UnityEvent<int> { }

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private HealthUpdateEvent updateHealthEvent;

    public int CurrHealthPoints { get; private set; }

    public HealthUpdateEvent UpdateHealthEvent { get => updateHealthEvent; }

    private void Awake()
    {
        CurrHealthPoints = maxHealthPoints;
    }

    public void SetMax()
    {
        CurrHealthPoints = maxHealthPoints;
        updateHealthEvent.Invoke(CurrHealthPoints);
    }

    public void Decrement()
    {
        CurrHealthPoints = Mathf.Max(0, CurrHealthPoints - 1);
        updateHealthEvent.Invoke(CurrHealthPoints);
    }

    public bool IsZero()
    {
        return CurrHealthPoints == 0;
    }
}
