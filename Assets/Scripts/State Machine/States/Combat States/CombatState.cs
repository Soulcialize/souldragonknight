using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;

namespace CombatStates
{
    public abstract class CombatState : State
    {
        protected readonly Combat owner;

        public CombatState(Combat owner)
        {
            this.owner = owner;
        }
    }
}
