using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GroundMovementStates;
using Photon.Pun;

public class MountInteractable : Interactable
{
    [SerializeField] private PhotonView photonView;
    
    [Space(10)]

    [SerializeField] private Transform mount;
    [SerializeField] private Movement mountMovement;
    [SerializeField] private Vector2 localOffset;

    [Space(10)]

    [SerializeField] private string mountedSortingLayerName;
    [SerializeField] private int mountedSortingLayerOrder;

    [Space(10)]

    // meant to be invoked on the mount's client only
    [SerializeField] private UnityEvent mountEvent;
    [SerializeField] private UnityEvent dismountEvent;

    private GroundMovement currentRiderMovement = null;

    public override void Interact(ActorController initiator)
    {
        if (initiator.Movement is GroundMovement groundMovement)
        {
            if (groundMovement.MovementStateMachine.CurrState is MountedState)
            {
                // dismount
                currentRiderMovement = null;
                groundMovement.Dismount();
                photonView.RPC("RPC_Dismount", RpcTarget.Others);
            }
            else
            {
                // mount
                currentRiderMovement = groundMovement;
                groundMovement.Mount(mount, mountMovement, localOffset, mountedSortingLayerName, mountedSortingLayerOrder);
                photonView.RPC("RPC_Mount", RpcTarget.Others);
            }
        }
    }

    [PunRPC]
    private void RPC_Mount()
    {
        // executed on mount's client
        mountEvent.Invoke();
    }

    [PunRPC]
    private void RPC_Dismount()
    {
        // executed on mount's client
        dismountEvent.Invoke();
    }

    [PunRPC]
    private void RPC_DismountMountedRider()
    {
        // executed on rider's client
        if (currentRiderMovement != null)
        {
            currentRiderMovement.Dismount();
            currentRiderMovement = null;
        }
    }

    /// <summary>
    /// Handles event where mount dies.
    /// </summary>
    /// <remarks>This method executes on the mount's client only.</remarks>
    public void MountDeathHandler()
    {
        // rider is separate client, so use RPC to dismount
        photonView.RPC("RPC_DismountMountedRider", RpcTarget.Others);
        dismountEvent.Invoke();
    }
}
