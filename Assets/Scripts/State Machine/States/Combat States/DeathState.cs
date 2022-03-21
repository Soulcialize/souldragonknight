using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class DeathState : CombatState
    {
        private int originalCollisionLayer;
        private string originalSpriteSortingLayer;

        public DeathState(Combat owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log($"{owner.gameObject.name} died");

            owner.Debuff();

            originalCollisionLayer = owner.Collider2d.gameObject.layer;
            owner.Collider2d.gameObject.layer = LayerMask.NameToLayer("Dead");

            owner.SpriteLayer.SetLayer(SpriteLayer.Layer.DEAD);

            owner.Animator.SetBool("isDead", true);
        }

        public override void Execute()
        {
            
        }

        public override void Exit()
        {
            owner.Collider2d.gameObject.layer = originalCollisionLayer;
            owner.SpriteLayer.ResetLayer();
            owner.Animator.SetBool("isDead", false);
        }
    }
}
