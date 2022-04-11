using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class NodeOccupationManager : MonoBehaviour
    {
        private static NodeOccupationManager _instance;
        public static NodeOccupationManager Instance { get => _instance; }

        private HashSet<Node> occupiedNodes = new HashSet<Node>();

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
        }

        public bool IsNodeOccupied(Node node)
        {
            return occupiedNodes.Contains(node);
        }

        public void MarkNodeAsOccupied(Node node)
        {
            occupiedNodes.Add(node);
        }

        public void MarkNodeAsUnoccupied(Node node)
        {
            occupiedNodes.Remove(node);
        }
    }
}
