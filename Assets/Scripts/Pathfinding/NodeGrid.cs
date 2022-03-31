using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class NodeGrid : MonoBehaviour
    {
        private static readonly int axisAlignedDistanceBetweenNeighbours = 10;
        private static readonly int diagonalDistanceBetweenNeighbours = 14;

        /// <summary>Minimum interval that individual classes should use when updating the grid.</summary>
        public static readonly float MIN_GRID_UPDATE_INTERVAL = 0.1f;

        private static NodeGrid _instance;
        public static NodeGrid Instance { get => _instance; }

        [SerializeField] private Vector2 center;
        [SerializeField] private Vector2 worldSize;
        [SerializeField] private float nodeRadius;
        [SerializeField] private LayerMask surfacesLayerMask;

        [Space(10)]

        [SerializeField] private Transform seeker;
        [SerializeField] private Transform target;

        private Node[,] grid;
        private float nodeDiameter;
        private int gridSizeX, gridSizeY;
        private Vector2 nodeBoxWalkableTester;

        private List<Node> path;

        private void Awake()
        {
            // singleton
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }

            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(worldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(worldSize.y / nodeDiameter);
            nodeBoxWalkableTester = new Vector2(nodeDiameter * 0.9f, nodeDiameter * 0.9f);

            CreateGrid();
            SetGridNodesNeighbours();
        }

        private void Update()
        {
            path = Pathfinder.FindPath(seeker.position, target.position, this);
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
                    CalculateNodeInfo(x, y, worldPoint, out bool isWalkable, out float distanceFromSurfaceBelow);
                    grid[x, y] = new Node(worldPoint, x, y, isWalkable, distanceFromSurfaceBelow);

                    /*
                    if (isWalkable)
                    {
                        GeneralUtility.CreateWorldTextObject(
                            $"{grid[x, y].GridX}, {grid[x, y].GridY}", worldPoint, transform, distanceFromSurfaceBelow.ToString("F2"));
                    }
                    */
                }
            }
        }

        private void CalculateNodeInfo(int nodeGridX, int nodeGridY, Vector2 worldPoint, out bool isWalkable, out float distanceFromSurfaceBelow)
        {
            isWalkable = Physics2D.OverlapBox(worldPoint, nodeBoxWalkableTester, 0f, surfacesLayerMask) == null;
            distanceFromSurfaceBelow = 0f;
            if (isWalkable)
            {
                if (nodeGridY > 0 && grid[nodeGridX, nodeGridY - 1].IsWalkable)
                {
                    // add on to existing distance of node directly below this one
                    distanceFromSurfaceBelow = grid[nodeGridX, nodeGridY - 1].DistanceFromSurfaceBelow + nodeDiameter;
                }
                else
                {
                    // no walkable node below this one; use raycast to find distance to surface below
                    RaycastHit2D raycastDownHit = Physics2D.Raycast(worldPoint, Vector2.down, Mathf.Infinity, surfacesLayerMask);
                    if (raycastDownHit.collider != null)
                    {
                        distanceFromSurfaceBelow = raycastDownHit.distance;
                    }
                }
            }
        }

        private void SetGridNodesNeighbours()
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Node node = grid[x, y];
                    node.SetNeighbours(GetNodeNeighbours(node));
                }
            }
        }

        private List<Node> GetNodeNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int xModifier = -1; xModifier <= 1; xModifier++)
            {
                for (int yModifier = -1; yModifier <= 1; yModifier++)
                {
                    if (xModifier == 0 && yModifier == 0)
                    {
                        // current node
                        continue;
                    }

                    int neighbourGridX = node.GridX + xModifier;
                    int neighbourGridY = node.GridY + yModifier;
                    if (neighbourGridX >= 0 && neighbourGridX < gridSizeX
                        && neighbourGridY >= 0 && neighbourGridY < gridSizeY)
                    {
                        // neighbouring node is inside grid
                        neighbours.Add(grid[neighbourGridX, neighbourGridY]);
                    }
                }
            }

            return neighbours;
        }

        public Node GetNodeFromWorldPoint(Vector2 worldPos)
        {
            float percentX = ((worldPos.x - center.x) / worldSize.x) + 0.5f;
            float percentY = ((worldPos.y - center.y) / worldSize.y) + 0.5f;

            int x = Mathf.FloorToInt(Mathf.Clamp(percentX * gridSizeX, 0, gridSizeX - 1));
            int y = Mathf.RoundToInt(Mathf.Clamp(percentY * gridSizeY, 0, gridSizeY - 1));

            return grid[x, y];
        }

        public int GetGridDistance(Node from, Node to)
        {
            int distanceX = Mathf.Abs(from.GridX - to.GridX);
            int distanceY = Mathf.Abs(from.GridY - to.GridY);
            if (distanceX > distanceY)
            {
                return distanceY * diagonalDistanceBetweenNeighbours +
                    (distanceX - distanceY) * axisAlignedDistanceBetweenNeighbours;
            }
            else
            {
                return distanceX * diagonalDistanceBetweenNeighbours +
                    (distanceY - distanceX) * axisAlignedDistanceBetweenNeighbours;
            }
        }

        public void UpdateGridRegion(Vector2 regionMinPoint, Vector2 regionMaxPoint)
        {
            Node minNode = GetNodeFromWorldPoint(regionMinPoint);
            Node maxNode = GetNodeFromWorldPoint(regionMaxPoint);

            for (int x = minNode.GridX; x <= maxNode.GridX; x++)
            {
                for (int y = minNode.GridY; y <= maxNode.GridY; y++)
                {
                    CalculateNodeInfo(x, y, grid[x, y].WorldPos, out bool isWalkable, out float distanceFromSurfaceBelow);
                    grid[x, y].UpdateInfo(isWalkable, distanceFromSurfaceBelow);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (grid == null)
            {
                return;
            }

            Color walkableColor = Color.white;
            Color unwalkableColor = new Color(0, 0, 0, 0.5f);
            foreach (Node node in grid)
            {
                if (path != null && path.Contains(node))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(node.WorldPos, Vector2.one * nodeDiameter);
                }
                else if (node.IsWalkable)
                {
                    Gizmos.color = walkableColor;
                    Gizmos.DrawWireCube(node.WorldPos, Vector2.one * nodeDiameter);
                }
                else
                {
                    Gizmos.color = unwalkableColor;
                    Gizmos.DrawCube(node.WorldPos, Vector2.one * nodeDiameter);
                }
            }
        }
    }
}
