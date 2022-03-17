using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GroundMovementStates;
using Photon.Pun;

public class MountInteractable : Interactable
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Transform mount;
    [SerializeField] private Movement mountMovement;
    [SerializeField] private Vector2 localOffset;
    [SerializeField] private string mountedSortingLayerName;
    [SerializeField] private int mountedSortingLayerOrder;

    private GroundMovement currentRiderMovement = null;

    public override void Interact(ActorController initiator)
    {
        if (initiator.Movement is GroundMovement groundMovement)
        {
            if (groundMovement.MovementStateMachine.CurrState is MountedState)
            {
                currentRiderMovement = null;
                groundMovement.Dismount();
            }
            else
            {
                currentRiderMovement = groundMovement;
                groundMovement.Mount(mount, mountMovement, localOffset, mountedSortingLayerName, mountedSortingLayerOrder);
            }
        }
    }

    [PunRPC]
    private void RPC_DismountMountedRider()
    {
        currentRiderMovement.Dismount();
        currentRiderMovement = null;
    }

    public void MountDeathHandler()
    {
        // rider is separate client, so use RPC to dismount
        photonView.RPC("RPC_DismountMountedRider", RpcTarget.Others);
    }
}
