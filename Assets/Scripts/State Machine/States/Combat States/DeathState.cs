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

            owner.Debuff();
            owner.CollisionLayer.SetLayer(LayerMask.NameToLayer("Dead"));
            owner.SpriteLayer.SetLayer(SpriteLayer.Layer.DEAD);
            owner.Animator.SetBool("isDead", true);
        }

        public override void Execute()
        {
            
        }

        public override void Exit()
        {
            owner.CollisionLayer.ResetLayer();
            owner.SpriteLayer.ResetLayer();
            owner.Animator.SetBool("isDead", false);
        }
    }
}
