using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Node
    {
        private Vector2 worldPos;
        private int gridX, gridY;

        public Vector2 WorldPos { get => worldPos; }

        public Node(Vector2 worldPos, int gridX, int gridY)
        {
            this.worldPos = worldPos;
            this.gridX = gridX;
            this.gridY = gridY;
        }
    }
}
