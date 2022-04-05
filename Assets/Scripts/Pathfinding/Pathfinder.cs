using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public static class Pathfinder
    {
        public static (bool, List<Node>) FindPath(
            NodeGrid grid, Vector2 fromPos, Vector2 toPos,
            (List<NodeNeighbourFilter> hardFilters, List<NodeNeighbourFilter> softFilters) filters)
        {
            Node fromNode = grid.GetNodeFromWorldPoint(fromPos);
            Node toNode = grid.GetNodeFromWorldPoint(toPos);

            if (fromNode.WorldPos == toNode.WorldPos)
            {
                return (true, RetracePath(fromNode, toNode));
            }

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(fromNode);

            Node currentNode = null;
            Node nodeWithLowestHCost = null;
            HashSet<Node> nonPreferredNeighbours = new HashSet<Node>();
            while (openSet.Count > 0)
            {
                // set the current node to the node with the lowest f_cost in the open set
                currentNode = GetNodeWithLowestFCost(openSet);
                if (nodeWithLowestHCost == null || currentNode.HCost < nodeWithLowestHCost.HCost)
                {
                    nodeWithLowestHCost = currentNode;
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == toNode)
                {
                    // found path
                    return (true, RetracePath(fromNode, toNode));
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

                        if (!openSet.Contains(neighbour))
                        {
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
                }

                if (openSet.Count == 0 && nonPreferredNeighbours.Count > 0)
                {
                    // no path can be found without using viable neighbours that don't pass the soft filters
                    openSet.AddRange(nonPreferredNeighbours);
                    nonPreferredNeighbours.Clear();
                }
            }

            // could not find path to target node; return path to the closest node we could find
            return (false, RetracePath(fromNode, nodeWithLowestHCost));
        }

        private static Node GetNodeWithLowestFCost(List<Node> nodeList)
        {
            if (nodeList.Count == 0)
            {
                throw new ArgumentException($"There needs to be at least 1 node in the given list");
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
            return path;
        }
    }
}
