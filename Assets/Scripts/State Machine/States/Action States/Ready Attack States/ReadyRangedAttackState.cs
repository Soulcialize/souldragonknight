using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatStates
{
    public class ReadyRangedAttackState : ReadyAttackState
    {
        private readonly Transform target;
        private readonly ProjectilePathDisplay projectilePathDisplay;
        private readonly ProjectileLauncher projectileLauncher;
        private readonly float lockTargetPositionTime;

        public bool HasLockedTargetPosition { get; private set; }
        public Vector2 TargetPosition { get; private set; }

        public ReadyRangedAttackState(
            Combat owner, Transform target,
            ProjectilePathDisplay projectilePathDisplay, ProjectileLauncher projectileLauncher,
            float lockTargetPositionTime, float readyDuration,
            UnityAction<Combat> readyCallback) : base(owner, readyDuration, readyCallback)
        {
            this.target = target;
            this.projectilePathDisplay = projectilePathDisplay;
            this.projectileLauncher = projectileLauncher;
            this.lockTargetPositionTime = lockTargetPositionTime;

            HasLockedTargetPosition = false;
        }

        public override void Enter()
        {
            base.Enter();
            projectilePathDisplay.StartDrawingProjectilePath(target);
            if (projectileLauncher != null)
            {
                projectileLauncher.StartAimingProjectileLauncher(target);
            }

            AudioManagerSynced.Instance.PlaySoundFx(false, owner.SoundFXIndexLibrary.ReadyAttack);
        }

        public override void Execute()
        {
            base.Execute();
            if (!HasLockedTargetPosition && Time.time - StartTime >= lockTargetPositionTime)
            {
                LockTargetPosition();
            }
        }

        public override void Exit()
        {
            base.Exit();
            projectilePathDisplay.StopDrawingProjectilePath();
            AudioManagerSynced.Instance.StopSoundFx(false, owner.SoundFXIndexLibrary.ReadyAttack);
        }

        private void LockTargetPosition()
        {
            Debug.Log($"{owner.name}: target position locked");
            HasLockedTargetPosition = true;
            TargetPosition = target.position;
            projectilePathDisplay.StopUpdatingProjectilePath();
            if (projectileLauncher != null)
            {
                projectileLauncher.StopAimingProjectileLauncher();
            }
        }
    }
}
