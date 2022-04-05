using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

using Filter = System.Predicate<Pathfinding.Node>;

public class PathfindingUnit : MonoBehaviour
{
    public static readonly float MIN_PATHFIND_INTERVAL = 0.5f;

    [SerializeField] private Collider2D collider2d;
    [SerializeField] private Movement movement;

    private Transform unitTransform;
    private int unitHeightInNodes;
    private (List<Filter> hardFilters, List<Filter> softFilters) filters;

    private float timeOfLastPathfind;

    private Coroutine pathfindProcess;
    private List<Node> path;
    private int targetNodeIndex;

    public int HeightInNodes { get => unitHeightInNodes; }

    private void Awake()
    {
        unitTransform = transform;
        unitHeightInNodes = Mathf.CeilToInt(collider2d.bounds.size.y / NodeGrid.Instance.NodeDiameter);
        timeOfLastPathfind = 0f;
        targetNodeIndex = 0;
    }

    public void SetFilters ((List<Filter>, List<Filter>) filters)
    {
        this.filters = filters;
    }

    /// <summary>
    /// Moves the unit along a path to the given target.
    /// </summary>
    /// <remarks>A strict minimum interval is kept between consecutive updates of the path.</remarks>
    /// <param name="targetPos">The target end point. It is expected to be the final center top of the unit's collider.</param>
    /// <param name="filters">Filters to apply to nodes while pathfinding.</param>
    /// <returns>True if path to target found. False otherwise.</returns>
    public bool Pathfind(Vector2 targetPos)
    {
        if (Time.time - timeOfLastPathfind < MIN_PATHFIND_INTERVAL)
        {
            return pathfindProcess != null;
        }

        StopPathfind();

        // factor in collider height while pathfinding
        bool heightFilter(Node node) => NodeGrid.Instance.AreNodesBelowWalkable(node, unitHeightInNodes - 1);
        filters.hardFilters.Add(heightFilter);
        List<Node> newPath = Pathfinder.FindPath(
            NodeGrid.Instance,
            GetCurrentPosForPathfinding(),
            targetPos,
            filters);

        if (newPath == null)
        {
            return false;
        }

        timeOfLastPathfind = Time.time;
        path = newPath;
        pathfindProcess = StartCoroutine(Pathfind());
        return true;
    }

    public void StopPathfind()
    {
        if (pathfindProcess != null)
        {
            StopCoroutine(pathfindProcess);
        }
    }

    private Vector2 GetCurrentPosForPathfinding()
    {
        return new Vector2(unitTransform.position.x, collider2d.bounds.max.y);
    }

    private Node GetCurrentPositionAsNode()
    {
        return NodeGrid.Instance.GetNodeFromWorldPoint(GetCurrentPosForPathfinding());
    }

    private IEnumerator Pathfind()
    {
        Node currentPosNode;
        targetNodeIndex = 0;
        while (targetNodeIndex < path.Count)
        {
            currentPosNode = GetCurrentPositionAsNode();
            if (currentPosNode == path[targetNodeIndex])
            {
                // reached current target nod; advance to next node
                targetNodeIndex++;
                if (targetNodeIndex >= path.Count)
                {
                    // reached final target node; pathfinding complete
                    yield break;
                }
            }

            Node currentTargetNode = path[targetNodeIndex];
            if (movement is GroundMovement groundMovement
                && currentTargetNode.WorldPos.y > currentPosNode.WorldPos.y)
            {
                groundMovement.Jump();
            }
            else
            {
                movement.UpdateMovement(currentTargetNode.WorldPos - currentPosNode.WorldPos);
            }

            yield return null;
        }

        // pathfinding complete
        StopPathfind();
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            // draw path
            Gizmos.color = Color.red;
            for (int i = targetNodeIndex; i < path.Count; i++)
            {
                Gizmos.DrawCube(path[i].WorldPos, Vector2.one * 0.2f);
                if (i == targetNodeIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i].WorldPos);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1].WorldPos, path[i].WorldPos);
                }
            }
        }
    }
}
