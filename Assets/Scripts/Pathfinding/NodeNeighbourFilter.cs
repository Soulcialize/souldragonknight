using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    using Filter = System.Func<Node, Node, bool>;

    public class NodeNeighbourFilter
    {
        private readonly Filter filter;

        public NodeNeighbourFilter(Filter filter)
        {
            this.filter = filter;
        }

        public bool DoesNodePass(Node node, Node neighbour)
        {
            return filter(node, neighbour);
        }
    }
}
