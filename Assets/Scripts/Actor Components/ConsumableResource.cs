using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

[System.Serializable]
public class UpdateResourceEvent : UnityEvent<float> { }
[System.Serializable]
public class RegenerateResourceEvent : UnityEvent<float> { }

public class ConsumableResource : MonoBehaviour
{
    [SerializeField] private float maxAmount;
    [SerializeField] private float delayBeforeRegen = 1.5f;
    [SerializeField] private float resourcePerSec = 10.0f;

    [SerializeField] private RegenerateResourceEvent regenerateResourceEvent;
    [SerializeField] private UpdateResourceEvent updateResourceEvent;
    [SerializeField] private PhotonView photonView;

    private Coroutine regen;

    public float CurrentAmount { get; private set; }

    public RegenerateResourceEvent RegenerateResourceEvent { get => regenerateResourceEvent; }
    public UpdateResourceEvent UpdateResourceEvent { get => updateResourceEvent; }

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
            CurrentAmount = Mathf.Max(CurrentAmount - amount, 0);
            float currentFill = CurrentAmount / maxAmount;
            updateResourceEvent.Invoke(currentFill);
        }
    }

    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(delayBeforeRegen);

        regenerateResourceEvent.Invoke(resourcePerSec / maxAmount);
        while (CurrentAmount < maxAmount)
        {
            CurrentAmount = Mathf.Min(maxAmount, 
                CurrentAmount + Time.deltaTime * resourcePerSec);
            yield return null;
        }
        regen = null;
    }
}
