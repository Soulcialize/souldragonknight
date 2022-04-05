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
    [SerializeField] private UnityEvent stopRegenResourceEvent;
    [SerializeField] private UnityEvent insufficientResourceEvent;
    [SerializeField] private PhotonView photonView;

    private Coroutine regen;

    public float CurrentAmount { get; private set; }

    public RegenerateResourceEvent RegenerateResourceEvent { get => regenerateResourceEvent; }
    public UpdateResourceEvent UpdateResourceEvent { get => updateResourceEvent; }
    public UnityEvent StopRegenResourceEvent { get => stopRegenResourceEvent; }
    public UnityEvent InsufficientResourceEvent { get => insufficientResourceEvent; }

    private void Start()
    {
        CurrentAmount = maxAmount;
    }

    public bool CanConsume(float amount)
    {
        if (CurrentAmount - amount >= 0)
        {
            return true;
        } 
        else
        {
            AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.INSUFFICIENT_RESOURCE);
            insufficientResourceEvent.Invoke();
            return false;
        }
    }

    public void Consume(float amount)
    {
        if (amount > 0)
        {
            if (regen != null)
            {
                StopCoroutine(regen);
            }

            regen = StartCoroutine(RegenerateWithDelay());
            CurrentAmount = Mathf.Max(CurrentAmount - amount, 0);
            float currentFill = CurrentAmount / maxAmount;
            updateResourceEvent.Invoke(currentFill);
        }
    }

    public void EmptyAndStopRegen()
    {
        CurrentAmount = 0.0f;
        updateResourceEvent.Invoke(0.0f);
        StopRegeneration();
    }

    public void StopRegeneration()
    {
        if (regen != null)
        {
            StopCoroutine(regen);
            regen = null;
        }
        stopRegenResourceEvent.Invoke();
    }

    public void Regenerate()
    {
        regen = StartCoroutine(RegenerateImmediately());
    }

    public void RestartRegeneration()
    {
        Debug.Log("Regenerate with delay");
        regen = StartCoroutine(RegenerateWithDelay());
    }

    private IEnumerator RegenerateImmediately()
    {
        regenerateResourceEvent.Invoke(resourcePerSec / maxAmount);
        while (CurrentAmount < maxAmount)
        {
            CurrentAmount = Mathf.Min(maxAmount,
                CurrentAmount + Time.deltaTime * resourcePerSec);
            yield return null;
        }
        regen = null;
    }

    private IEnumerator RegenerateWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeRegen);

        regen = StartCoroutine(RegenerateImmediately());
    }
}
