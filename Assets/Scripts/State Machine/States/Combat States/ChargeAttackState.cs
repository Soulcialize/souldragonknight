using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class ChargeAttackState : AttackState
    {
        private new readonly ChargeCombat owner;
        private readonly Transform ownerTransform;
        private readonly Vector2 targetPosition;
        private readonly string chargeCollisionLayerName;
        private readonly int originalLayer;

        private Vector2 startPos;
        private Vector2 chargeDirection;
        private float chargeDistance;

        private ActorController actorHit;

        public ChargeAttackState(ChargeCombat owner, Vector2 targetPosition, string chargeCollisionLayerName) : base(owner)
        {
            this.owner = owner;
            ownerTransform = owner.transform;
            this.targetPosition = targetPosition;
            this.chargeCollisionLayerName = chargeCollisionLayerName;
            originalLayer = owner.gameObject.layer;
        }

        public override void Enter()
        {
            base.Enter();
            startPos = ownerTransform.position;
            chargeDirection = (targetPosition - (Vector2)ownerTransform.position).normalized;
            chargeDistance = Vector2.Distance(ownerTransform.position, targetPosition);
            owner.gameObject.layer = LayerMask.NameToLayer(chargeCollisionLayerName);
        }

        public override void Execute()
        {
            base.Execute();
            owner.Rigidbody2d.velocity = chargeDirection * owner.ChargeSpeed;
            if (Vector2.Distance(startPos, ownerTransform.position) >= chargeDistance)
            {
                owner.Rigidbody2d.velocity = Vector2.zero;
                owner.CombatStateMachine.Exit();
            }
        }

        public override void Exit()
        {
            base.Exit();
            owner.Rigidbody2d.velocity = Vector2.zero;
            owner.gameObject.layer = originalLayer;
        }

        public override void ExecuteAttackEffect()
        {
            actorHit.Movement.UpdateMovement(Vector2.zero);
            actorHit.Combat.Hurt();
        }

        public void HandleCollision(Collision2D collision)
        {
            if (GeneralUtility.IsLayerInLayerMask(collision.gameObject.layer, owner.AttackEffectLayer))
            {
                actorHit = collision.gameObject.GetComponent<ActorController>();
                ExecuteAttackEffect();
            }

            owner.CombatStateMachine.Exit();
        }
    }
}
