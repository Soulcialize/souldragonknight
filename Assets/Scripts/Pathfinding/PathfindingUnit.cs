using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathfindingUnit : MonoBehaviour
{
    public static readonly float MIN_PATHFIND_INTERVAL = 0.1f;

    [SerializeField] private Collider2D collider2d;
    [SerializeField] private Movement movement;

    private Transform unitTransform;
    private int unitHeightInNodes;
    private (List<NodeNeighbourFilter> hardFilters, List<NodeNeighbourFilter> softFilters) filters;

    /// <summary>
    /// This list of soft filters is used for every unit alongside whatever gets passed during pathfinding.
    /// </summary>
    private List<NodeNeighbourFilter> commonHardNeighbourFilters;

    private float timeOfLastPathfind;

    private Coroutine pathfindProcess;
    private List<Node> path;
    private Pathfinder.PathfindResult lastPathfindResult;
    private int targetNodeIndex;

    public int HeightInNodes { get => unitHeightInNodes; }

    private void Awake()
    {
        unitTransform = transform;
        unitHeightInNodes = Mathf.CeilToInt(collider2d.bounds.size.y / NodeGrid.Instance.NodeDiameter);

        commonHardNeighbourFilters = new List<NodeNeighbourFilter>()
        {
            new NodeNeighbourFilter((node, neighbour) => NodeGrid.Instance.AreNodesBelowWalkable(neighbour, unitHeightInNodes - 1))
        };

        timeOfLastPathfind = 0f;
        lastPathfindResult = Pathfinder.PathfindResult.FAILURE;
        targetNodeIndex = 0;
    }

    public void SetFilters ((List<NodeNeighbourFilter>, List<NodeNeighbourFilter>) filters)
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
    public Pathfinder.PathfindResult Pathfind(Vector2 targetPos)
    {
        if (Time.time - timeOfLastPathfind < MIN_PATHFIND_INTERVAL)
        {
            return lastPathfindResult;
        }

        StopPathfind();

        // factor in collider height while pathfinding
        filters.hardFilters.AddRange(commonHardNeighbourFilters);
        (Pathfinder.PathfindResult result, List<Node> newPath) = Pathfinder.FindPath(
            NodeGrid.Instance,
            GetCurrentPosForPathfinding(),
            targetPos,
            filters);

        path = newPath;
        lastPathfindResult = result;
        if (result == Pathfinder.PathfindResult.FAILURE)
        {
            // cannot reach or advance any nearer to target position
            return result;
        }

        timeOfLastPathfind = Time.time;
        pathfindProcess = StartCoroutine(Pathfind());
        return result;
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
                // reached current target node; advance to next node
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
            Gizmos.color = Color.green;
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
