using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;

    private HashSet<Interactable> interactablesInRange = new HashSet<Interactable>();

    public bool IsEnabled { get; private set; }
    public Interactable CurrentNearestInteractable { get; private set; }

    private void Awake()
    {
        SetIsEnabled(photonView.IsMine);
    }

    public void SetIsEnabled(bool isEnabled)
    {
        IsEnabled = isEnabled;
        if (IsEnabled)
        {
            UpdateNearestInteractable();
        }
        else if (!IsEnabled && CurrentNearestInteractable != null)
        {
            CurrentNearestInteractable.HidePrompt();
        }
    }

    public Interactable GetNearestInteractable()
    {
        if (interactablesInRange.Count == 0)
        {
            return null;
        }

        Vector2 currPosition = transform.position;
        Interactable nearestInteractable = null;
        float nearestInteractableDistance = Mathf.Infinity;
        foreach (Interactable interactable in interactablesInRange)
        {
            if (!interactable.IsEnabled)
            {
                continue;
            }

            float distanceToInteractable = Vector2.Distance(currPosition, interactable.transform.position);
            if (distanceToInteractable < nearestInteractableDistance)
            {
                nearestInteractable = interactable;
                nearestInteractableDistance = distanceToInteractable;
            }
        }

        return nearestInteractable;
    }

    private void UpdateNearestInteractable()
    {
        Interactable nearestInteractable = GetNearestInteractable();

        // no nearby interactables
        if (nearestInteractable == null)
        {
            if (CurrentNearestInteractable != null)
            {
                // previous nearest interactable has gone out of range
                CurrentNearestInteractable.HidePrompt();
                CurrentNearestInteractable = nearestInteractable;
            }

            return;
        }

        // found nearest interactable
        if (nearestInteractable == CurrentNearestInteractable)
        {
            // newly found nearest interactable is the same as the current nearest one
            CurrentNearestInteractable.DisplayPrompt();
            return;
        }

        // update current nearest interactable to newly found one

        if (CurrentNearestInteractable != null)
        {
            CurrentNearestInteractable.HidePrompt();
        }

        CurrentNearestInteractable = nearestInteractable;
        CurrentNearestInteractable.DisplayPrompt();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsEnabled)
        {
            return;
        }

        Interactable[] interactables = collision.GetComponents<Interactable>();
        foreach (Interactable interactable in interactables)
        {
            interactablesInRange.Add(interactable);
        }

        UpdateNearestInteractable();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsEnabled)
        {
            return;
        }

        Interactable[] interactables = collision.GetComponents<Interactable>();
        foreach (Interactable interactable in interactables)
        {
            interactablesInRange.Remove(interactable);
        }

        UpdateNearestInteractable();
    }
}
