using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Node
    {
        public Vector2 WorldPos { get; private set; }
        public int GridX { get; private set; }
        public int GridY { get; private set; }

        public bool IsWalkable { get; private set; }
        public float DistanceFromSurfaceBelow { get; private set; }

        public List<Node> Neighbours { get; private set; }

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get => GCost + HCost; }

        public Node Parent { get; set; }

        public Node(Vector2 worldPos, int gridX, int gridY, bool isWalkable, float distanceFromSurfaceBelow)
        {
            WorldPos = worldPos;
            GridX = gridX;
            GridY = gridY;
            
            UpdateTraversalData(isWalkable, distanceFromSurfaceBelow);
        }

        public Node(SerializedNode serializedNode)
        {
            WorldPos = serializedNode.WorldPos;
            GridX = serializedNode.GridX;
            GridY = serializedNode.GridY;

            UpdateTraversalData(serializedNode.IsWalkable, serializedNode.DistanceFromSurfaceBelow);
        }

        public void SetNeighbours(List<Node> neighbours)
        {
            // neighbours can only be set after the rest of the grid's nodes have been created
            Neighbours = neighbours;
        }

        public void UpdateTraversalData(bool isWalkable, float distanceFromSurfaceBelow)
        {
            IsWalkable = isWalkable;
            DistanceFromSurfaceBelow = distanceFromSurfaceBelow;
        }
    }
}
