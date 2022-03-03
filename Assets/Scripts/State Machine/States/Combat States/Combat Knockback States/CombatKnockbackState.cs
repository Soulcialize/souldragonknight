using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public abstract class CombatKnockbackState : CombatState
    {
        private readonly Vector2 direction;

        private Vector2 startPos;

        protected bool hasKnockbackEnded = false;

        public CombatKnockbackState(Combat owner, Vector2 direction) : base(owner)
        {
            this.direction = direction.normalized;
        }

        public override void Enter()
        {
            startPos = owner.transform.position;
        }

        public override void Execute()
        {
            if (!hasKnockbackEnded)
            {
                owner.Rigidbody2d.velocity = direction * owner.KnockbackSpeed;
                if (owner.WallCollisionDetector.IsInContact)
                {
                    EndKnockback();
                    owner.Stun();
                }
                else if (Vector2.Distance(startPos, owner.transform.position) > owner.KnockbackDistance)
                {
                    EndKnockback();
                }
            }
        }

        public override void Exit()
        {

        }

        protected virtual void EndKnockback()
        {
            hasKnockbackEnded = true;
            owner.Rigidbody2d.velocity = Vector2.zero;
        }
    }
}
