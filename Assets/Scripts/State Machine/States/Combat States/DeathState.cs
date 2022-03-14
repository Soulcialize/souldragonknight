using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class DeathState : CombatState
    {
        private int originalCollisionLayer;

        public DeathState(Combat owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log($"{owner.gameObject.name} died");

            originalCollisionLayer = owner.Collider.gameObject.layer;
            owner.Collider.gameObject.layer = LayerMask.NameToLayer("Dead");
            owner.Animator.SetBool("isDead", true);
        }

        public override void Execute()
        {
            
        }

        public override void Exit()
        {
            owner.Collider.gameObject.layer = originalCollisionLayer;
            owner.Animator.SetBool("isDead", false);
        }
    }
}
