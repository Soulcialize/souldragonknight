using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public abstract class Interactable : MonoBehaviour
{
    public enum Interaction
    {
        MOUNT, REVIVE
    }

    [SerializeField] protected PhotonView photonView;
    [SerializeField] protected bool isEnabledByDefault;
    [SerializeField] protected float duration;

    private Coroutine interactionCoroutine;

    public bool IsEnabled { get; protected set; }
    public bool IsInteracting { get; private set; }
    public abstract Interaction InteractableInteraction { get; }

    protected virtual void Awake()
    {
        IsEnabled = isEnabledByDefault;
    }

    public abstract void Interact(ActorController initiator, UnityAction endInteractionCallback);

    public void StartInteraction(ActorController initiator, UnityAction endInteractionCallback)
    {
        IsInteracting = true;
        if (duration > 0f)
        {
            interactionCoroutine = StartCoroutine(ProcessInteraction(initiator, endInteractionCallback));
        }
        else
        {
            Interact(initiator, endInteractionCallback);
            IsInteracting = false;
        }
    }

    public void InterruptInteraction()
    {
        if (interactionCoroutine != null)
        {
            StopCoroutine(interactionCoroutine);
            interactionCoroutine = null;
            IsInteracting = false;
        }
    }

    public void SetIsEnabledWithoutSync(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }

    public void SetIsEnabledWithSync(bool isEnabled)
    {
        photonView.RPC("RPC_SetInteractableIsEnabled", RpcTarget.All, InteractableInteraction, isEnabled);
    }

    private IEnumerator ProcessInteraction(ActorController initiator, UnityAction endInteractionCallback)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Interact(initiator, endInteractionCallback);
        IsInteracting = false;
    }
}
