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

    [Header("UI")]

    [SerializeField] private InteractableUi interactableUi;
    [SerializeField] private string promptText;
    [SerializeField] private Vector2 localPosition;

    [Header("Events")]

    [SerializeField] private UnityEvent enableStatusUpdateEvent;

    private Coroutine interactionCoroutine;

    public bool IsEnabled { get; protected set; }
    public bool IsInteracting { get; private set; }
    public abstract Interaction InteractableInteraction { get; }

    public UnityEvent EnableStatusUpdateEvent { get => enableStatusUpdateEvent; }

    protected virtual void Awake()
    {
        IsEnabled = isEnabledByDefault;
    }

    private void OnDestroy()
    {
        enableStatusUpdateEvent.RemoveAllListeners();
    }

    public abstract void Interact(ActorController initiator, UnityAction endInteractionCallback);

    public void StartInteraction(ActorController initiator, UnityAction endInteractionCallback)
    {
        IsInteracting = true;
        if (duration > 0f)
        {
            initiator.Movement.UpdateMovement(Vector2.zero);
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
            interactableUi.SetInteractionProgressFill(0f);
            interactionCoroutine = null;
            IsInteracting = false;
        }
    }

    public void SetIsEnabledWithoutSync(bool isEnabled)
    {
        IsEnabled = isEnabled;
        enableStatusUpdateEvent.Invoke();
    }

    public void SetIsEnabledWithSync(bool isEnabled)
    {
        photonView.RPC("RPC_SetInteractableIsEnabled", RpcTarget.All, InteractableInteraction, isEnabled);
    }

    public void DisplayPrompt()
    {
        interactableUi.DisplayPrompt(promptText, duration, localPosition);
    }

    public void HidePrompt()
    {
        interactableUi.HidePrompt();
    }

    private IEnumerator ProcessInteraction(ActorController initiator, UnityAction endInteractionCallback)
    {
        float elapsedTime = 0f;
        interactableUi.SetInteractionProgressFill(0f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            interactableUi.SetInteractionProgressFill(elapsedTime / duration);
            yield return null;
        }

        interactableUi.SetInteractionProgressFill(1f);
        IsInteracting = false;
        Interact(initiator, endInteractionCallback);
    }
}
