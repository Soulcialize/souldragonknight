using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;

    private HashSet<Interactable> interactablesInRange = new HashSet<Interactable>();

    public Interactable CurrentNearestInteractable { get; private set; }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Interactable[] interactables = collision.GetComponents<Interactable>();
        foreach (Interactable interactable in interactables)
        {
            interactablesInRange.Add(interactable);
        }

        Interactable nearestInteractable = GetNearestInteractable();
        if (CurrentNearestInteractable == nearestInteractable)
        {
            return;
        }

        if (CurrentNearestInteractable != null)
        {
            CurrentNearestInteractable.HidePrompt();
        }

        CurrentNearestInteractable = nearestInteractable;
        CurrentNearestInteractable.DisplayPrompt();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Interactable[] interactables = collision.GetComponents<Interactable>();
        foreach (Interactable interactable in interactables)
        {
            if (CurrentNearestInteractable == interactable)
            {
                CurrentNearestInteractable.HidePrompt();
                CurrentNearestInteractable = null;
            }

            interactablesInRange.Remove(interactable);
        }
    }
}
