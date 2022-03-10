using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class BlockState : CombatState
    {
        private readonly float startTime;

        public float StartTime { get => startTime; }

        public BlockState(Combat owner) : base(owner)
        {
            startTime = Time.time;
        }

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

        public void Knockback(Vector2 direction)
        {
            owner.CombatStateMachine.ChangeState(new BlockKnockbackState(owner, direction));
        }
    }
}
