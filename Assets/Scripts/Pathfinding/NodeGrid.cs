using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class NodeGrid : MonoBehaviour
    {
        [SerializeField] private Vector2 center;
        [SerializeField] private Vector2 worldSize;
        [SerializeField] private float nodeRadius;

        private Node[,] grid;
        private float nodeDiameter;
        private int gridSizeX, gridSizeY;

        private void Awake()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(worldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(worldSize.y / nodeDiameter);
            CreateGrid();
        }

        private void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];

            Vector2 worldBottomLeft = center + Vector2.left * worldSize.x / 2 + Vector2.down * worldSize.y / 2;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                    grid[x, y] = new Node(worldPoint, x, y);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (grid == null)
            {
                return;
            }

            foreach (Node node in grid)
            {
                Gizmos.DrawWireCube(node.WorldPos, Vector2.one * nodeDiameter);
            }
        }
    }
}
