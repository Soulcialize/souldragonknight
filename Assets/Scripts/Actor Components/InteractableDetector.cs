using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    private HashSet<Interactable> interactablesInRange = new HashSet<Interactable>();

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
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactablesInRange.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactablesInRange.Remove(interactable);
        }
    }
}
