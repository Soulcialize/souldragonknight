using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class ReadyChargeAttackState : ReadyAttackState
    {
        private readonly Transform target;

        public bool HasLockedTargetPosition { get; private set; }
        public Vector2 TargetPosition { get; private set; }

        public ReadyChargeAttackState(Combat owner, Transform target) : base(owner)
        {
            this.target = target;
            HasLockedTargetPosition = false;
        }

        public void LockTargetPosition()
        {
            Debug.Log($"{owner.name}: target position locked");
            HasLockedTargetPosition = true;
            TargetPosition = target.position;
        }
    }
}
