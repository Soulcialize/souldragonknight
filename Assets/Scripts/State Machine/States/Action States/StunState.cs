using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class StunState : ActionState
    {
        public StunState(Combat owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log($"{owner.gameObject} stunned");
            owner.Animator.SetBool("isStunned", true);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.Animator.SetBool("isStunned", false);
        }
    }
}
