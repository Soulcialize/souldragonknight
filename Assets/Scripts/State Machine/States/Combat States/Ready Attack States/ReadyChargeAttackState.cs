using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class ReadyChargeAttackState : ReadyAttackState
    {
        private new readonly ChargeCombat owner;
        private readonly Transform target;

        private float startTime;

        public bool HasLockedTargetPosition { get; private set; }
        public Vector2 TargetPosition { get; private set; }

        public ReadyChargeAttackState(ChargeCombat owner, Transform target) : base(owner)
        {
            this.owner = owner;
            this.target = target;
            HasLockedTargetPosition = false;
        }

        public override void Enter()
        {
            base.Enter();
            startTime = Time.time;
        }

        public override void Execute()
        {
            base.Execute();
            owner.Rigidbody2d.velocity = Vector2.zero;
            if (!HasLockedTargetPosition && Time.time - startTime >= owner.LockTargetPositionTime)
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
