using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GroundMovementStates;

public class MountInteractable : Interactable
{
    public override void Interact(ActorController initiator)
    {
        if (initiator.Movement is GroundMovement groundMovement)
        {
            groundMovement.MovementStateMachine.ChangeState(new MountedState(groundMovement));
        }
    }
}
