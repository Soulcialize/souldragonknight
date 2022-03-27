using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GroundMovementStates
{
    public class MountedState : GroundMovementState
    {
        private readonly Movement mountMovement;
        private readonly MountInteractable mountInteractable;

        public MountedState(GroundMovement owner, Movement mountMovement, MountInteractable mountInteractable) : base(owner)
        {
            this.mountMovement = mountMovement;
            this.mountInteractable = mountInteractable;
        }

        public override void Enter()
        {
            owner.Rigidbody2d.velocity = Vector2.zero;
            if (owner.IsFacingRight != mountMovement.IsFacingRight)
            {
                owner.FlipDirection(owner.IsFacingRight ? Movement.Direction.LEFT : Movement.Direction.RIGHT);
            }
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.Rigidbody2d.velocity = Vector2.zero;
        }

        public void Dismount()
        {
            mountInteractable.Dismount(owner);
        }
    }
}
