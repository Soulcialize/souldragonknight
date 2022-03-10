using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class DodgeState : CombatState
    {
        private readonly Vector2 direction;
        private readonly float speed;
        private readonly float distance;

        private Vector2 startPos;

        public DodgeState(Combat owner, Vector2 direction, float speed, float distance) : base(owner)
        {
            this.direction = direction.normalized;
            this.speed = speed;
            this.distance = distance;
        }

        public override void Enter()
        {
            startPos = owner.transform.position;
            owner.Animator.SetBool("isDodging", true);
        }

        public override void Execute()
        {
            owner.Rigidbody2d.velocity = direction * speed;
            if (owner.WallCollisionDetector.IsInContact
                || Vector2.Distance(startPos, owner.transform.position) > distance)
            {
                EndDodge();
            }
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isDodging", false);
        }

        private void EndDodge()
        {
            owner.Rigidbody2d.velocity = Vector2.zero;
            owner.CombatStateMachine.Exit();
        }
    }
}
