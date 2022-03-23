using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class MountInteractable : Interactable
{
    [Header("Mount")]

    [SerializeField] private Transform mount;
    [SerializeField] private Movement mountMovement;
    [SerializeField] private Vector2 localOffset;

    [Space(10)]

    [SerializeField] private SpriteLayer.Layer mountedSpriteLayer;
    [SerializeField] private int mountedSpriteLayerOrder;

    [Space(10)]

    // meant to be invoked on the mount's client only
    [SerializeField] private UnityEvent mountEvent;
    [SerializeField] private UnityEvent dismountEvent;

    private GroundMovement currentRiderMovement = null;

    public override Interaction InteractableInteraction { get => Interaction.MOUNT; }

    public override void Interact(ActorController initiator, UnityAction endInteractionCallback)
    {
        if (!(initiator.Movement is GroundMovement groundMovement))
        {
            endInteractionCallback();
            return;
        }

        if (initiator.Movement.MovementStateMachine.CurrState is GroundMovementStates.MountedState)
        {
            currentRiderMovement = null;
            groundMovement.Dismount();
            photonView.RPC("RPC_Dismount", RpcTarget.Others);
        }
        else
        {
            // mount
            currentRiderMovement = groundMovement;
            groundMovement.Mount(mount, mountMovement, localOffset, mountedSpriteLayer, mountedSpriteLayerOrder);
            photonView.RPC("RPC_Mount", RpcTarget.Others);
        }

        endInteractionCallback();
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
