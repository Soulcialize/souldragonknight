using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class BlockState : CombatState
    {
        private readonly float startTime;

        public float StartTime { get => startTime; }

        public BlockState(MeleeCombat owner) : base(owner)
        {
            startTime = Time.time;
        }

        public BlockState(MeleeCombat owner, float startTime) : base(owner)
        {
            this.startTime = startTime;
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
    }
}
