using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatStates
{
    public class ChargeAttackState : AttackState
    {
        private readonly Transform ownerTransform;
        private readonly Vector2 targetPosition;
        private readonly float speed;
        private readonly float recoveryTime;
        private readonly UnityEvent chargeEndEvent;

        private Vector2 startPos;
        private Vector2 chargeDirection;
        private float chargeDistance;

        private bool hasChargeEnded = false;
        private float timeSinceChargeEnded = 0f;

        private ActorController actorHit;

        public ChargeAttackState(
            Combat owner, Vector2 targetPosition,
            float speed, float recoveryTime, UnityEvent chargeEndEvent) : base(owner)
        {
            ownerTransform = owner.transform;
            this.targetPosition = targetPosition;
            this.speed = speed;
            this.recoveryTime = recoveryTime;
            this.chargeEndEvent = chargeEndEvent;
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
                if (timeSinceChargeEnded > recoveryTime)
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
                owner.Rigidbody2d.velocity = chargeDirection * speed;
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
            chargeEndEvent.Invoke();
        }

        public override void ExecuteAttackEffect()
        {
            actorHit.Movement.UpdateMovement(Vector2.zero);
            actorHit.Combat.Hurt();
        }

        public void HandleCollision(Collision2D collision)
        {
            if (hasChargeEnded)
            {
                return;
            }

            if ( GeneralUtility.IsLayerInLayerMask(collision.gameObject.layer, owner.AttackEffectLayer))
            {
                actorHit = ActorController.GetActorFromCollider(collision.collider);
                ExecuteAttackEffect();
            }

            EndCharge();
        }
    }
}
