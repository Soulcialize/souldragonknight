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

        private Vector2 startPos;
        private Vector2 chargeDirection;
        private float chargeDistance;

        private bool hasChargeEnded = false;
        private float timeSinceChargeEnded = 0f;

        private ActorController actorHit;

        public ChargeAttackState(ChargeCombat owner, Vector2 targetPosition) : base(owner)
        {
            this.owner = owner;
            ownerTransform = owner.transform;
            this.targetPosition = targetPosition;
        }

        public override void Enter()
        {
            base.Enter();
            
            startPos = ownerTransform.position;
            chargeDirection = (targetPosition - (Vector2)ownerTransform.position).normalized;
            chargeDistance = Vector2.Distance(ownerTransform.position, targetPosition);
        }

        public override void Execute()
        {
            base.Execute();

            if (hasChargeEnded)
            {
                if (timeSinceChargeEnded > owner.ChargeRecoveryTime)
                {
                    owner.CombatStateMachine.Exit();
                }
                else
                {
                    timeSinceChargeEnded += Time.deltaTime;
                }
            }
            else
            {
                owner.Rigidbody2d.velocity = chargeDirection * owner.ChargeSpeed;
                if (Vector2.Distance(startPos, ownerTransform.position) >= chargeDistance)
                {
                    EndCharge();
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void EndCharge()
        {
            hasChargeEnded = true;
            owner.Rigidbody2d.velocity = Vector2.zero;
            owner.ChargeEndEvent.Invoke();
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
                actorHit = ActorController.GetActorFromCollider(collision.collider);
                ExecuteAttackEffect();
            }

            EndCharge();
        }
    }
}
