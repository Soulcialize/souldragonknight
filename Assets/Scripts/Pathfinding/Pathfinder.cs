using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public static class Pathfinder
    {
        public static List<Node> FindPath(NodeGrid grid, Vector2 fromPos, Vector2 toPos, params Predicate<Node>[] filters)
        {
            Node fromNode = grid.GetNodeFromWorldPoint(fromPos);
            Node toNode = grid.GetNodeFromWorldPoint(toPos);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(fromNode);

            HashSet<Node> nodesFilteredOut = new HashSet<Node>();
            while (openSet.Count > 0)
            {
                // set the current node to the node with the lowest f_cost in the open set
                Node currentNode = GetNodeWithLowestFCost(openSet);

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == toNode)
                {
                    // found path
                    return RetracePath(fromNode, toNode);
                }

                foreach (Node neighbour in currentNode.Neighbours)
                {
                    if (!neighbour.IsWalkable || closedSet.Contains(neighbour) || nodesFilteredOut.Contains(neighbour))
                    {
                        continue;
                    }

                    // keep node only if it passes the given filters
                    if (!DoesNodePassFilters(neighbour, filters))
                    {
                        // record filtered-out node so we can skip testing it in future iterations
                        nodesFilteredOut.Add(neighbour);
                        continue;
                    }

                    int newCostToNeighbour = currentNode.GCost + grid.GetGridDistance(currentNode, neighbour);
                    if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = grid.GetGridDistance(neighbour, toNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }

            // Debug.Log($"Could not find path from {fromPos} to {toPos}");
            return null;
        }

        private static Node GetNodeWithLowestFCost(List<Node> nodeList)
        {
            if (nodeList.Count == 0)
            {
                throw new System.ArgumentException($"There needs to be at least 1 node in the given list");
            }

            Node currentNode = nodeList[0];
            for (int i = 0; i < nodeList.Count; i++)
            {
                // if the two nodes have the same f_cost, use the node with the lower h_cost
                if (nodeList[i].FCost < currentNode.FCost
                    || nodeList[i].FCost == currentNode.FCost && nodeList[i].HCost < currentNode.HCost)
                {
                    currentNode = nodeList[i];
                }
            }

            return currentNode;
        }

        private static bool DoesNodePassFilters(Node node, Predicate<Node>[] filters)
        {
            foreach (Predicate<Node> filter in filters)
            {
                if (!filter(node))
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
            return path;
        }
    }
}
