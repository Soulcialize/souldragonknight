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
    [SerializeField] private Combat mountCombat;
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

    protected override void Interact(ActorController initiator, UnityAction endInteractionCallback)
    {
        if (!(initiator.Movement is GroundMovement groundMovement)
            || initiator.Movement.MovementStateMachine.CurrState is GroundMovementStates.MountedState)
        {
            // only actors who primarily move on the ground and are not mounted can start mounting
            endInteractionCallback();
            return;
        }

        Mount(groundMovement);
        endInteractionCallback();
    }

    public void Mount(GroundMovement riderGroundMovement)
    {
        // executed on rider's client
        currentRiderMovement = riderGroundMovement;
        SetIsEnabledWithSync(false);
        riderGroundMovement.Mount(
            mount, mountMovement, this,
            mountMovement.IsFacingRight ? localOffset : new Vector2(-localOffset.x, localOffset.y),
            mountedSpriteLayer, mountedSpriteLayerOrder);

        photonView.RPC("RPC_Mount", RpcTarget.Others, currentRiderMovement.NetworkViewId);
    }

    public void Dismount(GroundMovement riderGroundMovement)
    {
        // executed on rider's client
        currentRiderMovement = null;
        SetIsEnabledWithSync(true);
        riderGroundMovement.Dismount();
        photonView.RPC("RPC_Dismount", RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_Mount(int riderMovementViewId)
    {
        // executed on mount's client
        currentRiderMovement = PhotonView.Find(riderMovementViewId).GetComponent<GroundMovement>();
        mountCombat.ToggleCombatAbilities(false);
        mountEvent.Invoke();
    }

    [PunRPC]
    private void RPC_Dismount()
    {
        // executed on mount's client
        currentRiderMovement = null;
        mountCombat.ToggleCombatAbilities(true);
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

    private void FlipMountedRider()
    {
        if (photonView.IsMine)
        {
            // execute only one client since Movement.FlipDirection uses an RPC call of its own
            currentRiderMovement.FlipDirection(
                mountMovement.IsFacingRight ? Movement.Direction.RIGHT : Movement.Direction.LEFT);
        }

        Vector3 riderLocalPos = currentRiderMovement.transform.localPosition;
        riderLocalPos.x = -riderLocalPos.x;
        currentRiderMovement.transform.localPosition = riderLocalPos;
    }

    /// <summary>
    /// Handles event where mount flips direction.
    /// </summary>
    /// <remarks>This method executes on all clients.</remarks>
    public void MountFlipHandler()
    {
        if (currentRiderMovement != null)
        {
            FlipMountedRider();
        }   
    }

    /// <summary>
    /// Handles event where mount dies.
    /// </summary>
    /// <remarks>This method executes on the mount's client only.</remarks>
    public void MountDeathHandler()
    {
        // rider is separate client, so use RPC to dismount
        currentRiderMovement = null;
        mountCombat.ToggleCombatAbilities(true);
        photonView.RPC("RPC_DismountMountedRider", RpcTarget.Others);
    }
}
