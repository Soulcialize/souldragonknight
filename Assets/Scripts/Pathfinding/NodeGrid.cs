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

        [SerializeField] private PathfindingGridData precomputedGridData;

        [Space(10)]

        [SerializeField] private Transform seeker;
        [SerializeField] private Transform target;

        private Node[,] grid;
        private float nodeDiameter;
        private int gridSizeX, gridSizeY;
        private Vector2 nodeBoxWalkableTester;

        public List<Node> path;

        public Vector2 Center { get => center; }
        public Vector2 WorldSize { get => worldSize; }
        public float NodeRadius { get => nodeRadius; }
        public LayerMask SurfacesLayerMask { get => surfacesLayerMask; }

        public float NodeDiameter { get => nodeDiameter; }

        public PathfindingGridData PrecomputedGridData { get => precomputedGridData; }

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

            LoadPrecomputedGridData();
        }

        private void Update()
        {
            // path = Pathfinder.FindPath(this, seeker.position, target.position);
        }

        public static void CalculateNodeTraversalData(Vector2 worldPoint, float nodeDiameter,
            Vector2 nodeBoxWalkableTester, LayerMask surfacesLayerMask,
            bool isNodeBelowWalkable, float distanceFromNodeBelowToSurfaceBelow,
            out bool isWalkable, out float distanceFromSurfaceBelow)
        {
            // calculate if walkable
            isWalkable = Physics2D.OverlapBox(worldPoint, nodeBoxWalkableTester, 0f, surfacesLayerMask) == null;

            // calculate distance to surface below
            distanceFromSurfaceBelow = 0f;
            if (isWalkable)
            {
                if (isNodeBelowWalkable)
                {
                    // add on to existing distance of node directly below this one
                    distanceFromSurfaceBelow = distanceFromNodeBelowToSurfaceBelow + nodeDiameter;
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

        private void LoadPrecomputedGridData()
        {
            nodeDiameter = precomputedGridData.NodeDiameter;
            gridSizeX = precomputedGridData.GridSizeX;
            gridSizeY = precomputedGridData.GridSizeY;
            nodeBoxWalkableTester = precomputedGridData.NodeBoxWalkableTester;

            grid = new Node[gridSizeX, gridSizeY];

            // create nodes
            foreach (SerializedNode serializedNode in precomputedGridData.PrecomputedNodes)
            {
                grid[serializedNode.GridX, serializedNode.GridY] = new Node(serializedNode);
                // draw node data in scene
                Node node = grid[serializedNode.GridX, serializedNode.GridY];
                if (node.IsWalkable)
                {
                    GeneralUtility.CreateWorldTextObject(
                        $"{node.GridX}, {node.GridY}", node.WorldPos, transform, node.DistanceFromSurfaceBelow.ToString("F2"));
                }
            }

            // set nodes' neighbours
            foreach (SerializedNode serializedNode in precomputedGridData.PrecomputedNodes)
            {
                List<Node> neighbours = new List<Node>();
                foreach (Vector2 neighbourGridPos in serializedNode.Neighbours)
                {
                    neighbours.Add(grid[(int)neighbourGridPos.x, (int)neighbourGridPos.y]);
                }

                grid[serializedNode.GridX, serializedNode.GridY].SetNeighbours(neighbours);
            }
        }

        public Node GetNodeFromGridPos(int gridX, int gridY)
        {
            return grid[gridX, gridY];
        }

        public Node GetNodeFromWorldPoint(Vector2 worldPos)
        {
            float percentX = ((worldPos.x - center.x) / worldSize.x) + 0.5f;
            float percentY = ((worldPos.y - center.y) / worldSize.y) + 0.5f;

            int x = Mathf.FloorToInt(Mathf.Clamp(percentX * gridSizeX, 0, gridSizeX - 1));
            int y = Mathf.FloorToInt(Mathf.Clamp(percentY * gridSizeY, 0, gridSizeY - 1));

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

            bool isNodeBelowWalkable;
            float distanceFromNodeBelowToSurfaceBelow;
            for (int x = minNode.GridX; x <= maxNode.GridX; x++)
            {
                for (int y = minNode.GridY; y <= maxNode.GridY; y++)
                {
                    if (y > 0)
                    {
                        isNodeBelowWalkable = grid[x, y - 1].IsWalkable;
                        distanceFromNodeBelowToSurfaceBelow = grid[x, y - 1].DistanceFromSurfaceBelow;
                    }
                    else
                    {
                        isNodeBelowWalkable = false;
                        distanceFromNodeBelowToSurfaceBelow = 0f;
                    }

                    CalculateNodeTraversalData(
                        grid[x, y].WorldPos, nodeDiameter,
                        nodeBoxWalkableTester, surfacesLayerMask,
                        isNodeBelowWalkable, distanceFromNodeBelowToSurfaceBelow,
                        out bool isWalkable, out float distanceFromSurfaceBelow);
                    grid[x, y].UpdateTraversalData(isWalkable, distanceFromSurfaceBelow);
                }
            }
        }

        public int GetColliderHeightInNodes(Collider2D collider)
        {
            return Mathf.CeilToInt(collider.bounds.size.y / nodeDiameter);
        }

        public bool AreNodesBelowWalkable(Node node, int numToCheck)
        {
            for (int y = node.GridY - 1; y >= Mathf.Max(0, node.GridY - numToCheck); y--)
            {
                if (!grid[node.GridX, y].IsWalkable)
                {
                    return false;
                }
            }

            return true;
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
                    Gizmos.DrawWireCube(node.WorldPos, nodeBoxWalkableTester);
                }
                else
                {
                    Gizmos.color = unwalkableColor;
                    Gizmos.DrawCube(node.WorldPos, Vector2.one * nodeDiameter);
                    Gizmos.DrawWireCube(node.WorldPos, nodeBoxWalkableTester);
                }
            }
        }
    }
}
