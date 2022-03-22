using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReviveInteractable : Interactable
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Combat combat;

    public override void Interact(ActorController initiator)
    {
        photonView.RPC("RPC_Revive", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Revive()
    {
        // executed on dead actor's client
        if (combat.CombatStateMachine.CurrState is CombatStates.DeathState)
        {
            combat.Revive();
        }
    }
}
