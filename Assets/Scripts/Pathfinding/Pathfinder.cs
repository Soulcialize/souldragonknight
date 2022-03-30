using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public static class Pathfinder
    {
        public static List<Node> FindPath(Vector2 fromPos, Vector2 toPos, NodeGrid grid)
        {
            Node fromNode = grid.GetNodeFromWorldPoint(fromPos);
            Node toNode = grid.GetNodeFromWorldPoint(toPos);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(fromNode);

            while (openSet.Count > 0)
            {
                // set the current node to the node with the lowest f_cost in the open set
                // if two or more nodes have the same lowest f_cost in the open set, use the node with the lowest h_cost
                Node currentNode = openSet[0];
                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost
                        || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == toNode)
                {
                    // found path
                    return RetracePath(fromNode, toNode);
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
                            openSet.Add(neighbour);
                        }
                    }
                }
            }

            // Debug.Log($"Could not find path from {fromPos} to {toPos}");
            return null;
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
