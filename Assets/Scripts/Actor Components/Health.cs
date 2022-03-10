using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int healthPoints;

    public int HealthPoints { get => healthPoints; }

    public void Decrement()
    {
        healthPoints = Mathf.Max(0, healthPoints - 1);
    }

    public bool IsZero()
    {
        return healthPoints == 0;
    }
}
