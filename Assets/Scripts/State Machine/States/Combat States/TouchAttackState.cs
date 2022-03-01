using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatStates
{
    public class TouchAttackState : AttackState
    {
        public TouchAttackState(TouchCombat owner) : base(owner) { }

        public override void ExecuteAttackEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}
