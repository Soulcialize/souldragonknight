using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private GameObject[] dragonHealthUI;
    [SerializeField] private GameObject[] knightHealthUI;
    [SerializeField] private int dragonHealth;
    [SerializeField] private int knightHealth;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject life in dragonHealthUI)
        {
            life.SetActive(true);
        }
        foreach (GameObject life in knightHealthUI)
        {
            life.SetActive(true);
        }
    }

    public int DragonHealth { get => dragonHealth; }

    public int KnightHealth { get => knightHealth; }

    public void decrementDragonHealthUI()
    {
        dragonHealth = Mathf.Max(0, dragonHealth - 1);
        dragonHealthUI[dragonHealth].SetActive(false);
    }

    public void decrementKnightHealthUI()
    {
        knightHealth = Mathf.Max(0, knightHealth - 1);
        knightHealthUI[knightHealth].SetActive(false);
    }
}
