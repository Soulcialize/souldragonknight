using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Pathfinding
{
    public static class NodeGridInitializer
    {
        [MenuItem("Pathfinding/Initialize Grids")]
        private static void InitializeNodeGrid()
        {
            Scene activeScene = SceneManager.GetActiveScene();

            if (!EditorUtility.DisplayDialog(
                "Initializing Pathfinding Grids",
                $"Initializing Pathfinding Grids for the active scene ({activeScene.name}). Existing grid data in this scene will be overwritten. Proceed?",
                "Yes", "No"))
            {
                return;
            }

            Debug.Log($"Starting grids initialization for {activeScene.name}");

            // get all grids in scene
            List<NodeGrid> grids = new List<NodeGrid>();
            foreach (GameObject gameObject in activeScene.GetRootGameObjects())
            {
                NodeGrid nodeGrid = gameObject.GetComponent<NodeGrid>();
                if (nodeGrid != null)
                {
                    grids.Add(nodeGrid);
                }

                foreach (NodeGrid childNodeGrid in gameObject.GetComponentsInChildren<NodeGrid>())
                {
                    grids.Add(childNodeGrid);
                }
            }

            float nodeDiameter;
            int gridSizeX, gridSizeY;
            Vector2 nodeBoxWalkableTester;
            foreach (NodeGrid nodeGrid in grids)
            {
                // initialize grid data
                nodeDiameter = nodeGrid.NodeRadius * 2;
                gridSizeX = Mathf.RoundToInt(nodeGrid.WorldSize.x / nodeDiameter);
                gridSizeY = Mathf.RoundToInt(nodeGrid.WorldSize.y / nodeDiameter);
                nodeBoxWalkableTester = new Vector2(nodeDiameter * 0.1f, nodeDiameter * 0.1f);

                // precompute grid nodes with neighbours and save data to scriptable object
                SerializedNode[,] grid = PrecomputeGridNodes(nodeGrid, nodeDiameter, gridSizeX, gridSizeY, nodeBoxWalkableTester);
                SetGridNodesNeighbours(grid);
                SavePrecomputedGrid(nodeGrid, grid, nodeDiameter, nodeBoxWalkableTester);
            }

            Debug.Log($"Completed grids initialization for {activeScene.name}");
        }

        private static SerializedNode[,] PrecomputeGridNodes(
            NodeGrid nodeGrid, float nodeDiameter, int gridSizeX, int gridSizeY, Vector2 nodeBoxWalkableTester)
        {
            SerializedNode[,] grid = new SerializedNode[gridSizeX, gridSizeY];

            // iterate through each node's position from the bottom left of the grid
            Vector2 worldBottomLeft = nodeGrid.Center
                + Vector2.left * nodeGrid.WorldSize.x / 2
                + Vector2.down * nodeGrid.WorldSize.y / 2;

            SerializedNode nodeBelowCurrent = null;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    // find position of node in world space
                    Vector2 worldPoint = worldBottomLeft
                        + Vector2.right * (x * nodeDiameter + nodeGrid.NodeRadius)
                        + Vector2.up * (y * nodeDiameter + nodeGrid.NodeRadius);

                    // get traversal data of node below the current node
                    bool isNodeBelowWalkable = nodeBelowCurrent != null && nodeBelowCurrent.IsWalkable;
                    float distanceFromNodeBelowToSurfaceBelow = nodeBelowCurrent == null
                        ? 0f
                        : nodeBelowCurrent.DistanceFromSurfaceBelow;

                    // calculate traversal data of current node
                    NodeGrid.CalculateNodeTraversalData(
                        worldPoint, nodeDiameter,
                        nodeBoxWalkableTester, nodeGrid.SurfacesLayerMask,
                        isNodeBelowWalkable, distanceFromNodeBelowToSurfaceBelow,
                        out bool isWalkable, out float distanceFromSurfaceBelow);

                    // create serialized node
                    SerializedNode node = new SerializedNode(worldPoint, x, y, isWalkable, distanceFromSurfaceBelow);
                    grid[x, y] = node;

                    // set current node as node below the node in the next iteration
                    nodeBelowCurrent = y < gridSizeY - 1 ? node : null;
                }
            }

            return grid;
        }

        private static void SetGridNodesNeighbours(SerializedNode[,] grid)
        {
            // iterate through each node in the grid and set its neighbours
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    grid[x, y].SetNeighbours(GetSerializedNodeNeighbours(grid[x, y], grid));
                }
            }
        }

        private static List<Vector2> GetSerializedNodeNeighbours(SerializedNode node, SerializedNode[,] grid)
        {
            List<Vector2> neighbours = new List<Vector2>();

            // iterate through each of the eight possible positions a neighbour can be in
            for (int xModifier = -1; xModifier <= 1; xModifier++)
            {
                for (int yModifier = -1; yModifier <= 1; yModifier++)
                {
                    if (xModifier == 0 && yModifier == 0)
                    {
                        // this is the current node's position
                        continue;
                    }

                    int neighbourGridX = node.GridX + xModifier;
                    int neighbourGridY = node.GridY + yModifier;
                    if (neighbourGridX >= 0 && neighbourGridX < grid.GetLength(0)
                        && neighbourGridY >= 0 && neighbourGridY < grid.GetLength(1))
                    {
                        // neighbouring node is inside grid
                        neighbours.Add(new Vector2(neighbourGridX, neighbourGridY));
                    }
                }
            }

            return neighbours;
        }

        private static void SavePrecomputedGrid(
            NodeGrid nodeGrid, SerializedNode[,] grid, float nodeDiameter, Vector2 nodeBoxWalkableTester)
        {
            if (nodeGrid.PrecomputedGridData == null)
            {
                Debug.LogError($"{nodeGrid.gameObject} object does not have a reference " +
                    $"to a PathfindingGridData scriptable object for saving precomputed data");
                return;
            }

            string[] assetGuids = AssetDatabase.FindAssets(
                $"{nodeGrid.PrecomputedGridData.name} t:PathfindingGridData",
                new[] { "Assets/Scriptable Objects/Pathfinding" });

            // load asset from first GUID (there should only be one GUID in the array)
            PathfindingGridData gridData = (PathfindingGridData)AssetDatabase.LoadAssetAtPath(
                AssetDatabase.GUIDToAssetPath(assetGuids[0]),
                typeof(PathfindingGridData));

            // save data into scriptable object
            gridData.SavePrecomputedGridData(grid, nodeDiameter, nodeBoxWalkableTester);

            // save changes
            EditorUtility.SetDirty(gridData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
