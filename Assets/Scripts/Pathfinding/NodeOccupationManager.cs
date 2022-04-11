using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class NodeOccupationManager : MonoBehaviour
    {
        private static NodeOccupationManager _instance;
        public static NodeOccupationManager Instance { get => _instance; }

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
            return node.IsOccupied;
        }

        public void MarkNodeAsOccupied(Node node)
        {
            node.IsOccupied = true;
        }

        public void MarkNodeAsUnoccupied(Node node)
        {
            node.IsOccupied = false;
        }
    }
}
