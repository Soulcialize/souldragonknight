using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GroundMovementStates
{
    public class MountedState : GroundMovementState
    {
        private readonly Transform mount;

        public MountedState(GroundMovement owner, Transform mount) : base(owner)
        {
            this.mount = mount;
        }

        public override void Enter()
        {
            owner.Mount(mount);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {

        }
    }
}
