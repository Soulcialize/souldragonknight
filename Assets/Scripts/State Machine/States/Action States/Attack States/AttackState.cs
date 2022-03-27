using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public abstract class AttackState : ActionState
    {
        public AttackState(Combat owner) : base(owner) { }

        public override void Enter()
        {
            owner.Animator.SetBool("isAttacking", true);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.Animator.SetBool("isAttacking", false);
        }

        public abstract void ExecuteAttackEffect();
    }
}
