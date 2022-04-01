using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    [CreateAssetMenu(fileName = "New Pathfinding Grid Data", menuName = "Scriptable Objects/Pathfinding Grid Data")]
    public class PathfindingGridData : ScriptableObject
    {
        [SerializeField] private int gridSizeX;
        [SerializeField] private int gridSizeY;
        [SerializeField] private float nodeDiameter;
        [SerializeField] private Vector2 nodeBoxWalkableTester;
        [SerializeField] private List<SerializedNode> precomputedNodes;

        public int GridSizeX { get => gridSizeX; }
        public int GridSizeY { get => gridSizeY; }
        public float NodeDiameter { get => nodeDiameter; }
        public Vector2 NodeBoxWalkableTester { get => nodeBoxWalkableTester; }
        public List<SerializedNode> PrecomputedNodes { get => precomputedNodes; }

        public void SavePrecomputedGridData(SerializedNode[,] grid, float nodeDiameter, Vector2 nodeBoxWalkableTester)
        {
            this.nodeDiameter = nodeDiameter;
            this.nodeBoxWalkableTester = nodeBoxWalkableTester;

            gridSizeX = grid.GetLength(0);
            gridSizeY = grid.GetLength(1);

            // 2D array cannot be serialized, so we store the grid's nodes as a list
            precomputedNodes = new List<SerializedNode>();
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    precomputedNodes.Add(grid[x, y]);
                }
            }
        }
    }
}
