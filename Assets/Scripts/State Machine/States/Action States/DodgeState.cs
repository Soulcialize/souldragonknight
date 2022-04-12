using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class DodgeState : ActionState
    {
        private readonly Vector2 direction;
        private readonly float speed;
        private readonly float distance;

        private Vector2 startPos;
        private float startTime;
        private float maxDodgeTime;

        public DodgeState(Combat owner, Vector2 direction, float speed, float distance) : base(owner)
        {
            this.direction = direction.normalized;
            this.speed = speed;
            this.distance = distance;
        }

        public override void Enter()
        {
            startPos = owner.transform.position;
            startTime = Time.time;
            maxDodgeTime = distance / speed;
            owner.Animator.SetBool("isDodging", true);
            AudioManagerSynced.Instance.PlaySoundFx(true, owner.SoundFXIndexLibrary.Dodge);
        }

        public override void Execute()
        {
            owner.Rigidbody2d.velocity = direction * speed;
            if (Vector2.Distance(startPos, owner.transform.position) > distance
                || Time.time - startTime > maxDodgeTime)
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
            owner.ActionStateMachine.Exit();
        }
    }
}
