using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class BlockState : CombatState
    {
        public BlockState(MeleeCombat owner) : base(owner) { }

        public override void Enter()
        {
            owner.Animator.SetBool("isBlocking", true);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.Animator.SetBool("isBlocking", false);
        }
    }
}
