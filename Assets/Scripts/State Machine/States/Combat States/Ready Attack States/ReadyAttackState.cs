using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class ReadyAttackState : CombatState
    {
        public float StartTime { get; protected set; }

        public ReadyAttackState(Combat owner) : base(owner) { }

        public override void Enter()
        {
            StartTime = Time.time;
            owner.Animator.SetBool("isReadyingAttack", true);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.Animator.SetBool("isReadyingAttack", false);
        }
    }
}
