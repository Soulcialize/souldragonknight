using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class ReviveInteractable : Interactable
{
    [SerializeField] private Combat combat;

    public override Interaction InteractableInteraction { get => Interaction.REVIVE; }

    public override void Interact(ActorController initiator, UnityAction endInteractionCallback)
    {
        photonView.RPC("RPC_Revive", RpcTarget.All);
        endInteractionCallback();
    }

    [PunRPC]
    private void RPC_Revive()
    {
        if (combat.CombatStateMachine.CurrState is CombatStates.DeathState)
        {
            // should only execute on dead actor's client
            combat.Revive();
        }
    }
}
