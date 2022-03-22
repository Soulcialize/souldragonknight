using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

[System.Serializable]
public class ConsumeResourceEvent : UnityEvent<float> { }

public class ConsumableResource : MonoBehaviour
{
    [SerializeField] private float maxAmount;
    [SerializeField] private UnityEvent regenerateResourceEvent;
    [SerializeField] private ConsumeResourceEvent consumeResourceEvent;
    [SerializeField] private PhotonView photonView;

    private readonly float delayBeforeRegen = 1.5f;
    private readonly WaitForSeconds regenTick = new WaitForSeconds(0.1f);

    private Coroutine regen;

    public float CurrentAmount { get; private set; }

    public UnityEvent RegenerateResourceEvent { get => regenerateResourceEvent; }
    public ConsumeResourceEvent ConsumeResourceEvent { get => consumeResourceEvent; }

    private void Start()
    {
        CurrentAmount = maxAmount;
    }

    public bool CanConsume(float amount)
    {
        return CurrentAmount - amount >= 0;
    }

    public void Consume(float amount)
    {
        if (amount > 0)
        {
            if (regen != null)
            {
                StopCoroutine(regen);
            }

            regen = StartCoroutine(Regenerate());
            CurrentAmount -= amount;
            float currentFill = CurrentAmount / maxAmount;
            consumeResourceEvent.Invoke(currentFill);
        }
    }

    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(delayBeforeRegen);

        regenerateResourceEvent.Invoke();
        while (CurrentAmount < maxAmount)
        {
            CurrentAmount = Mathf.Min(maxAmount, CurrentAmount + maxAmount / 50);
            yield return regenTick;
        }
        regen = null;
    }
}
