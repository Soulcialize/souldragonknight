using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeBlackboards;

namespace AiBehaviorTreeNodes
{
    /// <summary>
    /// Action node that sets the stored combat target's position as the navigation target.
    /// </summary>
    /// <remarks>
    /// <br><b>Success</b>: Always.</br>
    /// <br><b>Failure</b>: -</br>
    /// <br><b>Running</b>: -</br>
    /// </remarks>
    public class SetCombatTargetPosNode : BehaviorNode
    {
        private readonly Transform ownerTransform;
        private readonly Movement ownerMovement;
        private readonly Combat ownerCombat;

        public SetCombatTargetPosNode(Movement ownerMovement, Combat ownerCombat)
        {
            ownerTransform = ownerMovement.transform;
            this.ownerMovement = ownerMovement;
            this.ownerCombat = ownerCombat;
        }

        public override NodeState Execute()
        {
            ActorController target = (ActorController)Blackboard.GetData(CombatBlackboardKeys.COMBAT_TARGET);

            Vector2 navTargetPos = Vector2.zero;
            if (ownerMovement is GroundMovement)
            {
                navTargetPos = Physics2D.Raycast(
                    (Vector2)target.Combat.Collider2d.transform.position + target.Combat.Collider2d.offset,
                    Vector2.down, Mathf.Infinity, ownerMovement.GroundDetector.SurfacesLayerMask).point;
                navTargetPos = new Vector2(navTargetPos.x, navTargetPos.y + ownerCombat.Collider2d.bounds.extents.y);
            }
            else if (ownerMovement is AirMovement)
            {
                navTargetPos = (Vector2)target.Combat.Collider2d.transform.position + target.Combat.Collider2d.offset;
            }

            Blackboard.SetData(GeneralBlackboardKeys.NAV_TARGET, navTargetPos);
            Blackboard.SetData(
                GeneralBlackboardKeys.NAV_TARGET_STOPPING_DISTANCE,
                Mathf.Min(ownerMovement.DefaultStoppingDistanceFromNavTargets, target.Movement.DefaultStoppingDistanceFromNavTargets));

            FindPathToCombatTargetPosition(navTargetPos);
            return NodeState.SUCCESS;
        }

        private void FindPathToCombatTargetPosition(Vector2 targetPosition)
        {
            if (ownerMovement is AirMovement)
            {
                Blackboard.SetData(
                    GeneralBlackboardKeys.NAV_TARGET_PATH,
                    Pathfinding.Pathfinder.FindPath(
                        Pathfinding.NodeGrid.Instance,
                        ownerTransform.position,
                        targetPosition));
            }
            else if (ownerMovement is GroundMovement groundMovement)
            {
                RaycastHit2D groundHit = Physics2D.Raycast(ownerTransform.position, Vector2.down, Mathf.Infinity, ownerMovement.GroundDetector.SurfacesLayerMask);
                float maxDistanceFromGround = groundHit.distance + groundMovement.MaxReachableHeight;
                Blackboard.SetData(
                    GeneralBlackboardKeys.NAV_TARGET_PATH,
                    Pathfinding.Pathfinder.FindPath(
                        Pathfinding.NodeGrid.Instance,
                        ownerTransform.position,
                        targetPosition,
                        true,
                        node => node.DistanceFromSurfaceBelow <= maxDistanceFromGround));
            }
        }
    }
}
