using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiBehaviorTreeNodes
{
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
