using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class TouchAttackState : AttackState
    {
        private new readonly TouchCombat owner;
        private readonly Transform ownerTransform;
        private readonly Vector2 targetPosition;

        private Vector2 startPos;
        private Vector2 chargeDirection;
        private float chargeDistance;

        private ActorController actorHit;

        public TouchAttackState(TouchCombat owner, Vector2 targetPosition) : base(owner)
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
        }

        public override void ExecuteAttackEffect()
        {
            actorHit.Movement.UpdateMovement(Vector2.zero);
            actorHit.Combat.Hurt();
            owner.CombatStateMachine.Exit();
        }

        public void HandleCollision(Collision2D collision)
        {
            if (owner.AttackEffectLayer == (owner.AttackEffectLayer | (1 << collision.gameObject.layer)))
            {
                actorHit = collision.gameObject.GetComponent<ActorController>();
                ExecuteAttackEffect();
            }
            else
            {
                owner.CombatStateMachine.Exit();
            }
        }
    }
}
