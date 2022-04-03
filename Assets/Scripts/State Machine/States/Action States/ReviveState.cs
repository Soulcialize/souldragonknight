using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class ReviveState : ActionState
    {
        public ReviveState(Combat owner) : base(owner) { }

        public override void Enter()
        {
            AudioManagerSynced.Instance.PlaySoundFx(owner.SoundFXIndexLibrary.Revive);
            owner.Health.SetMax();
            owner.SpriteLayer.ResetLayer();
            owner.Animator.SetBool("isReviving", true);
            owner.Resource.Regenerate();
            owner.ReviveStartEvent.Invoke();
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            owner.CollisionLayer.ResetLayer();
            owner.Animator.SetBool("isReviving", false);
            owner.ReviveFinishEvent.Invoke();
        }
    }
}
