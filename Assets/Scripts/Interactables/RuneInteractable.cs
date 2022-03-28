using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RuneInteractable : Interactable
{
    [SerializeField] private SpriteRenderer[] runes;

    public override Interaction InteractableInteraction { get => Interaction.SWITCH; }

    public override Interactor InteractableInteractor { get => Interactor.KNIGHT; }

    public override void Interact(ActorController initiator, UnityAction endInteractionCallback)
    {
        Debug.Log("switch");
    }
}
