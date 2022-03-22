using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class ReviveState : CombatState
    {
        public ReviveState(Combat owner) : base(owner) { }

        public override void Enter()
        {
            owner.Health.SetMax();
            owner.SpriteLayer.ResetLayer();
            owner.Animator.SetBool("isReviving", true);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.CollisionLayer.ResetLayer();
            owner.Animator.SetBool("isReviving", false);
        }
    }
}
