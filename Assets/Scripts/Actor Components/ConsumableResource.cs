using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableResource : MonoBehaviour
{
    [SerializeField] private float maxAmount;
    [SerializeField] private Slider resourceBar;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);

    public float CurrentAmount { get; private set; }
    
    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(2);

        while (CurrentAmount < maxAmount)
        {
            CurrentAmount += maxAmount / 100;
            resourceBar.value = CurrentAmount / maxAmount;
            yield return regenTick;
        }
    }
}
