using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that increments a time variable on the blackboard.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: Always.</br>
    /// <br><b>Failure</b>: -</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class UpdateTimeVariableNode : BehaviorNode
    {
        private readonly string variableName;

        public UpdateTimeVariableNode(string variableName)
        {
            this.variableName = variableName;
        }

        public override NodeState Execute()
        {
            Blackboard.SetData(variableName, (float)Blackboard.GetData(variableName, 0f) + Time.deltaTime);
            return NodeState.SUCCESS;
        }
    }
}
