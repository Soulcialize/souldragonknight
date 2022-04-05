using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public static class NodeTraversalCalculator
    {
        public static void CalculateNodeTraversalData(Vector2 worldPoint, float nodeDiameter,
            Vector2 nodeBoxWalkableTester, LayerMask surfacesLayerMask,
            bool isNodeBelowWalkable, float distanceFromNodeBelowToSurfaceBelow,
            out bool isWalkable, out float distanceFromSurfaceBelow)
        {
            // calculate if walkable
            isWalkable = IsNodeWalkable(worldPoint, nodeBoxWalkableTester, surfacesLayerMask);

            // calculate distance to surface below
            distanceFromSurfaceBelow = isWalkable
                ? CalculateDistanceToSurfaceBelow(
                    worldPoint, nodeDiameter, surfacesLayerMask, isNodeBelowWalkable, distanceFromNodeBelowToSurfaceBelow)
                : 0f;
        }

        private static bool IsNodeWalkable(Vector2 worldPoint, Vector2 nodeBoxWalkableTester, LayerMask surfacesLayerMask)
        {
            return Physics2D.OverlapBox(worldPoint, nodeBoxWalkableTester, 0f, surfacesLayerMask) == null;
        }

        private static float CalculateDistanceToSurfaceBelow(
            Vector2 worldPoint, float nodeDiameter, LayerMask surfacesLayerMask,
            bool isNodeBelowWalkable, float distanceFromNodeBelowToSurfaceBelow)
        {
            if (isNodeBelowWalkable)
            {
                // add on to existing distance of node directly below this one
                return distanceFromNodeBelowToSurfaceBelow + nodeDiameter;
            }
            else
            {
                // no walkable node directly below this one; use raycast to find distance to surface below
                RaycastHit2D raycastDownHit = Physics2D.Raycast(worldPoint, Vector2.down, Mathf.Infinity, surfacesLayerMask);
                return raycastDownHit.collider != null
                    ? raycastDownHit.distance
                    : Mathf.Infinity;
            }
        }
    }
}
