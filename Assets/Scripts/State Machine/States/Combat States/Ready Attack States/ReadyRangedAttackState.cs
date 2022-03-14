using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatStates
{
    public class ReadyRangedAttackState : ReadyAttackState
    {
        private readonly Transform target;
        private readonly float lockTargetPositionTime;

        public bool HasLockedTargetPosition { get; private set; }
        public Vector2 TargetPosition { get; private set; }

        public ReadyRangedAttackState(
            Combat owner, Transform target,
            float lockTargetPositionTime, float readyDuration,
            UnityAction<Combat> readyCallback) : base(owner, readyDuration, readyCallback)
        {
            this.target = target;
            this.lockTargetPositionTime = lockTargetPositionTime;

            HasLockedTargetPosition = false;
        }

        public override void Execute()
        {
            base.Execute();
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
