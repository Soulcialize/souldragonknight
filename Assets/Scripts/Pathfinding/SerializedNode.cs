using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    [System.Serializable]
    public class SerializedNode
    {
        [Header("Positional Data")]

        [SerializeField] private Vector2 worldPos;
        [SerializeField] private int gridX;
        [SerializeField] private int gridY;

        [Header("Traversal Data")]

        [SerializeField] private bool isWalkable;
        [SerializeField] private float distanceFromSurfaceBelow;

        [Header("Neighbours")]

        [SerializeField] private List<Vector2> neighbours;

        public Vector2 WorldPos { get => worldPos; }
        public int GridX { get => gridX; }
        public int GridY { get => gridY; }

        public bool IsWalkable { get => isWalkable; }
        public float DistanceFromSurfaceBelow { get => distanceFromSurfaceBelow; }

        public List<Vector2> Neighbours { get => neighbours; }

        public SerializedNode(Vector2 worldPos, int gridX, int gridY, bool isWalkable, float distanceFromSurfaceBelow)
        {
            this.worldPos = worldPos;
            this.gridX = gridX;
            this.gridY = gridY;
            this.isWalkable = isWalkable;
            this.distanceFromSurfaceBelow = distanceFromSurfaceBelow;
        }

        public void SetNeighbours(List<Vector2> neighbours)
        {
            this.neighbours = neighbours;
        }
    }
}
