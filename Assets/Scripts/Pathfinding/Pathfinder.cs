using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public static class Pathfinder
    {
        public enum PathfindResult
        {
            SUCCESS, // can reach given target
            FAILURE, // cannot move to any node that is nearer to the target than the current position
            NEAREST // cannot reach target, but can reach a node near to it
        }

        public static (PathfindResult, List<Node>) FindPath(
            NodeGrid grid, Vector2 fromPos, Vector2 toPos,
            (List<NodeNeighbourFilter> hardFilters, List<NodeNeighbourFilter> softFilters) filters)
        {
            Node fromNode = grid.GetNodeFromWorldPoint(fromPos);
            Node toNode = grid.GetNodeFromWorldPoint(toPos);

            if (fromNode.WorldPos == toNode.WorldPos)
            {
                return (PathfindResult.SUCCESS, RetracePath(fromNode, toNode));
            }

            Heap<Node> openSet = new Heap<Node>(grid.GridSizeX * grid.GridSizeY);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(fromNode);

            Node currentNode = fromNode;
            currentNode.GCost = 0;
            currentNode.HCost = grid.GetGridDistance(fromNode, toNode);

            Node nodeWithLowestHCost = fromNode;
            HashSet<Node> nonPreferredNeighbours = new HashSet<Node>();
            while (openSet.Count > 0)
            {
                // set the current node to the node with the lowest F cost in the open set
                currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);
                if (currentNode.HCost < nodeWithLowestHCost.HCost)
                {
                    nodeWithLowestHCost = currentNode;
                }

                if (currentNode == toNode)
                {
                    // found path
                    return (PathfindResult.SUCCESS, RetracePath(fromNode, toNode));
                }

                foreach (Node neighbour in currentNode.Neighbours)
                {
                    if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = currentNode.GCost + grid.GetGridDistance(currentNode, neighbour);
                    if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = grid.GetGridDistance(neighbour, toNode);
                        neighbour.Parent = currentNode;

                        if (openSet.Contains(neighbour))
                        {
                            openSet.UpdateItem(neighbour);
                            continue;
                        }

                        if (filters.hardFilters != null && !DoesNeighbourPassFilters(currentNode, neighbour, filters.hardFilters))
                        {
                            // neighbour doesn't pass hard filters
                            closedSet.Add(neighbour);
                        }
                        else if (filters.softFilters != null && !DoesNeighbourPassFilters(currentNode, neighbour, filters.softFilters))
                        {
                            // keep non-preferred neighbours in consideration in case no other path exists
                            nonPreferredNeighbours.Add(neighbour);
                        }
                        else
                        {
                            // neighbour passes all filters
                            openSet.Add(neighbour);
                        }
                    }
                }

                if (openSet.Count == 0 && nonPreferredNeighbours.Count > 0)
                {
                    // no path can be found without using viable neighbours that don't pass the soft filters
                    foreach (Node neighbour in nonPreferredNeighbours)
                    {
                        openSet.Add(neighbour);
                    }

                    nonPreferredNeighbours.Clear();
                }
            }

            // could not find path to target node; try to return path to the closest node we could find
            List<Node> closestPathToTarget = RetracePath(fromNode, nodeWithLowestHCost);
            return (closestPathToTarget.Count == 0 ? PathfindResult.FAILURE : PathfindResult.NEAREST, closestPathToTarget);
        }

        private static bool DoesNeighbourPassFilters(Node node, Node neighbour, List<NodeNeighbourFilter> filters)
        {
            foreach (NodeNeighbourFilter filter in filters)
            {
                if (!filter.DoesNodePass(node, neighbour))
                {
                    return false;
                }
            }

            return true;
        }

        private static List<Node> RetracePath(Node fromNode, Node toNode)
        {
            List<Node> path = new List<Node>();
            
            Node currentNode = toNode;
            while (currentNode != fromNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return SimplifyPath(path);
        }

        private static List<Node> SimplifyPath(List<Node> path)
        {
            if (path.Count == 0)
            {
                return path;
            }

            List<Node> simplifiedPath = new List<Node> { path[0] };

            Vector2 prevDirection = Vector2.zero;
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 currDirection = path[i].WorldPos - path[i - 1].WorldPos;
                if (currDirection != prevDirection)
                {
                    simplifiedPath.Add(path[i]);
                }

                prevDirection = currDirection;
            }

            return simplifiedPath;
        }
    }
}
