using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GroundMovementStates
{
    public class MountedState : GroundMovementState
    {
        private readonly Transform mount;
        private readonly Vector2 localOffset;
        private readonly string mountedSortingLayerName;
        private readonly int mountedSortingLayerOrder;

        private string originalSortingLayerName;
        private int originalSortingLayerOrder;

        public MountedState(
            GroundMovement owner, Transform mount, Vector2 localOffset,
            string mountedSortingLayerName, int mountedSortingLayerOrder) : base(owner)
        {
            this.mount = mount;
            this.localOffset = localOffset;
            this.mountedSortingLayerName = mountedSortingLayerName;
            this.mountedSortingLayerOrder = mountedSortingLayerOrder;
        }

        public override void Enter()
        {
            owner.Rigidbody2d.velocity = Vector2.zero;

            originalSortingLayerName = owner.SpriteRenderer.sortingLayerName;
            originalSortingLayerOrder = owner.SpriteRenderer.sortingOrder;
            
            owner.Mount(mount, localOffset, mountedSortingLayerName, mountedSortingLayerOrder);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.Rigidbody2d.velocity = Vector2.zero;
            owner.Dismount(originalSortingLayerName, originalSortingLayerOrder);
        }
    }
}
