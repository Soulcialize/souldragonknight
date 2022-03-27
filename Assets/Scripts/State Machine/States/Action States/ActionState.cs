using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;

namespace CombatStates
{
    public abstract class ActionState : State
    {
        protected readonly Combat owner;

        public ActionState(Combat owner)
        {
            this.owner = owner;
        }
    }
}
