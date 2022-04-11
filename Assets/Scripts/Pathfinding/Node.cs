using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Node : IHeapItem<Node>
    {
        private int heapIndex;

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

        public int HeapIndex { get => heapIndex; set => heapIndex = value; }

        public bool IsOccupied { get; set; }

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

        public int CompareTo(Node other)
        {
            int result = FCost.CompareTo(other.FCost);
            if (result == 0)
            {
                result = HCost.CompareTo(other.HCost);
            }

            // node with lower F Cost / H Cost should have higher priority
            return -result;
        }

        public override bool Equals(object obj)
        {
            Node otherNode = obj as Node;
            if (otherNode == null)
            {
                return false;
            }

            return GridX == otherNode.GridX && GridY == otherNode.GridY;
        }

        public override int GetHashCode()
        {
            return GridX.GetHashCode() ^ GridY.GetHashCode();
        }
    }
}
