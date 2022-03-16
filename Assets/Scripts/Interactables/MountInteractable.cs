using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GroundMovementStates;

public class MountInteractable : Interactable
{
    [SerializeField] private Transform mount;
    [SerializeField] private Vector2 localOffset;
    [SerializeField] private string mountedSortingLayerName;
    [SerializeField] private int mountedSortingLayerOrder;

    public override void Interact(ActorController initiator)
    {
        if (initiator.Movement is GroundMovement groundMovement)
        {
            if (groundMovement.MovementStateMachine.CurrState is MountedState)
            {
                groundMovement.MovementStateMachine.ChangeState(new AirborneState(groundMovement));
            }
            else
            {
                groundMovement.MovementStateMachine.ChangeState(new MountedState(
                    groundMovement, mount, localOffset, mountedSortingLayerName, mountedSortingLayerOrder));
            }
        }
    }
}
