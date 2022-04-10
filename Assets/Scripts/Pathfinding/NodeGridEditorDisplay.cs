using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    /// <summary>
    /// Displays the grid in the editor without traversal information.
    /// </summary>
    /// <remarks>
    /// The main use of the display is for editing the positions of objects in the scene editor.
    /// </remarks>
    public class NodeGridEditorDisplay : MonoBehaviour
    {
        [SerializeField] private NodeGrid nodeGrid;
        [SerializeField] private bool drawGrid;

        private void OnDrawGizmos()
        {
            if (!drawGrid)
            {
                return;
            }

            // find starting node in the bottom left of the grid
            Vector2 worldBottomLeft = nodeGrid.Center
                + Vector2.left * nodeGrid.WorldSize.x / 2
                + Vector2.down * nodeGrid.WorldSize.y / 2;

            Gizmos.color = Color.white;
            float nodeDiameter = nodeGrid.NodeRadius * 2f;
            int gridSizeX = Mathf.RoundToInt(nodeGrid.WorldSize.x / nodeDiameter);
            int gridSizeY = Mathf.RoundToInt(nodeGrid.WorldSize.y / nodeDiameter);

            Vector2 worldPoint;
            for (int x = 0; x < gridSizeX; x++)
            {   
                for (int y = 0; y < gridSizeY; y++)
                {
                    // find position of node in world space
                    worldPoint = worldBottomLeft
                        + Vector2.right * (x * nodeDiameter + nodeGrid.NodeRadius)
                        + Vector2.up * (y * nodeDiameter + nodeGrid.NodeRadius);

                    Gizmos.DrawWireCube(worldPoint, Vector2.one * nodeDiameter);
                }
            }
        }
    }
}
