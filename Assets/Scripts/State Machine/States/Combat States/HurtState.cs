using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class HurtState : CombatState
    {
        public HurtState(Combat owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log($"{owner.gameObject.name} hurt");
            owner.Animator.SetBool("isHurt", true);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.Animator.SetBool("isHurt", false);
        }
    }
}
