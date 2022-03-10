using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatStates
{
    public class ReadyChargeAttackState : ReadyAttackState
    {
        private readonly Transform target;
        private readonly float lockTargetPositionTime;
        private readonly UnityEvent readyChargeStartEvent;

        public bool HasLockedTargetPosition { get; private set; }
        public Vector2 TargetPosition { get; private set; }

        public ReadyChargeAttackState(
            Combat owner, Transform target,
            float lockTargetPositionTime, float readyDuration,
            UnityAction<Combat> readyCallback, UnityEvent readyChargeStartEvent) : base(owner, readyDuration, readyCallback)
        {
            this.target = target;
            this.lockTargetPositionTime = lockTargetPositionTime;
            this.readyChargeStartEvent = readyChargeStartEvent;

            HasLockedTargetPosition = false;
        }

        public override void Enter()
        {
            base.Enter();
            readyChargeStartEvent.Invoke();
        }

        public override void Execute()
        {
            base.Execute();
            owner.Rigidbody2d.velocity = Vector2.zero;
            if (!HasLockedTargetPosition && Time.time - StartTime >= lockTargetPositionTime)
            {
                LockTargetPosition();
            }
        }

        private void LockTargetPosition()
        {
            Debug.Log($"{owner.name}: target position locked");
            HasLockedTargetPosition = true;
            TargetPosition = target.position;
        }
    }
}
