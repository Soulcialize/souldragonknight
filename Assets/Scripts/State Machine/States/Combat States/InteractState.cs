using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class InteractState : CombatState
    {
        private readonly ActorController initiator;
        private readonly Interactable interactable;

        public InteractState(Combat owner, ActorController initiator, Interactable interactable) : base(owner)
        {
            this.initiator = initiator;
            this.interactable = interactable;
        }

        public override void Enter()
        {
            interactable.StartInteraction(initiator, EndInteractionCallback);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {

        }

        private void EndInteractionCallback()
        {
            owner.CombatStateMachine.Exit();
        }

        public void InterruptInteraction()
        {
            interactable.InterruptInteraction();
            owner.CombatStateMachine.Exit();
        }
    }
}
