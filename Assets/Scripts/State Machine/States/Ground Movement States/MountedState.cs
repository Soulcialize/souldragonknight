using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GroundMovementStates
{
    public class MountedState : GroundMovementState
    {
        private readonly Movement mountMovement;

        public string OriginalSortingLayerName { get; private set; }
        public int OriginalSortingLayerOrder { get; private set; }

        public MountedState(GroundMovement owner, Movement mountMovement) : base(owner)
        {
            this.mountMovement = mountMovement;
        }

        public override void Enter()
        {
            owner.Rigidbody2d.velocity = Vector2.zero;
            if (owner.IsFacingRight != mountMovement.IsFacingRight)
            {
                owner.FlipDirection(owner.IsFacingRight ? Movement.Direction.LEFT : Movement.Direction.RIGHT);
            }

            OriginalSortingLayerName = owner.SpriteRenderer.sortingLayerName;
            OriginalSortingLayerOrder = owner.SpriteRenderer.sortingOrder;
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.Rigidbody2d.velocity = Vector2.zero;
        }
    }
}
