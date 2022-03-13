using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class DeathState : CombatState
    {
        public DeathState(Combat owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log($"{owner.gameObject.name} died");
            owner.Animator.SetBool("isDead", true);
        }

        public override void Execute()
        {
            
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isDead", false);
        }
    }
}
