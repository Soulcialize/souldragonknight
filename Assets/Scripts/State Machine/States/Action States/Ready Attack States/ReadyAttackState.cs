using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CombatStates
{
    public class ReadyAttackState : ActionState
    {
        private readonly float duration;
        private readonly UnityAction<Combat> readyCallback;

        private bool hasReadied = false;

        public float StartTime { get; protected set; }

        public ReadyAttackState(Combat owner, float duration, UnityAction<Combat> readyCallback) : base(owner)
        {
            this.duration = duration;
            this.readyCallback = readyCallback;
        }

        public override void Enter()
        {
            AudioManagerSynced.Instance.PlaySoundFx(false, owner.SoundFXIndexLibrary.ReadyAttack);
            StartTime = Time.time;
            owner.Animator.SetBool("isReadyingAttack", true);
        }

        public override void Execute()
        {
            if (!hasReadied && Time.time - StartTime > duration)
            {
                hasReadied = true;
                readyCallback(owner);
            }
        }

        public override void Exit()
        {
            owner.Animator.SetBool("isReadyingAttack", false);
        }
    }
}
