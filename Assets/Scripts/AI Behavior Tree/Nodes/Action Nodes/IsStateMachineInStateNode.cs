using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;

namespace AiBehaviorTreeNodes
{
    public class IsStateMachineInStateNode : BehaviorNode
    {
        private readonly StateMachine stateMachine;
        private readonly System.Type stateType;

        public IsStateMachineInStateNode(StateMachine stateMachine, System.Type stateType)
        {
            if (!stateType.IsSubclassOf(typeof(State)))
            {
                throw new System.ArgumentException($"{stateType} is not a State");
            }

            this.stateMachine = stateMachine;
            this.stateType = stateType;
        }

        public override NodeState Execute()
        {
            if (stateMachine.CurrState == null)
            {
                return NodeState.FAILURE;
            }

            return stateType.IsAssignableFrom(stateMachine.CurrState.GetType())
                ? NodeState.SUCCESS
                : NodeState.FAILURE;
        }
    }
}
