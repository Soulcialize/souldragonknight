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
            interactable.StartInteraction(initiator, owner.CombatStateMachine.Exit);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            if (interactable.IsInteracting)
            {
                interactable.InterruptInteraction();
            }
        }

        public void InterruptInteraction()
        {
            owner.CombatStateMachine.Exit();
        }
    }
}
