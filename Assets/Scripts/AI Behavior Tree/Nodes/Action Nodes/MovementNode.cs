using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Abstract action node that can move an actor in a given direction.
    /// </summary>
    /// <remarks>
    /// With this class, inheriting action nodes that want to move an actor in a given direction
    /// can do so without having to differentiate between actors that use ground/aerial movement.
    /// </remarks>
    public abstract class MovementNode : BehaviorNode
    {
        protected readonly Movement ownerMovement;

        public MovementNode(Movement ownerMovement)
        {
            this.ownerMovement = ownerMovement;
        }

        protected void UpdateMovement(Vector2 direction)
        {
            if (ownerMovement is GroundMovement groundMovement)
            {
                groundMovement.UpdateHorizontalMovement(direction.x);
            }
            else if (ownerMovement is AirMovement airMovement)
            {
                airMovement.UpdateHorizontalMovement(direction.x);
                airMovement.UpdateVerticalMovement(direction.y);
            }
        }
    }
}
