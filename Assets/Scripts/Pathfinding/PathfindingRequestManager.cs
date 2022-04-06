using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pathfinding
{
    public class PathfindingRequestManager : MonoBehaviour
    {
        private struct PathfindingRequest
        {
            public readonly Vector2 fromPos;
            public readonly Vector2 toPos;
            public readonly (List<NodeNeighbourFilter>, List<NodeNeighbourFilter>) filters;
            public readonly UnityAction<Pathfinder.PathfindResult, List<Node>> callback;

            public PathfindingRequest(
                Vector2 fromPos, Vector2 toPos,
                (List<NodeNeighbourFilter>, List<NodeNeighbourFilter>) filters,
                UnityAction<Pathfinder.PathfindResult, List<Node>> callback)
            {
                this.fromPos = fromPos;
                this.toPos = toPos;
                this.filters = filters;
                this.callback = callback;
            }
        }

        private static PathfindingRequestManager _instance;
        public static PathfindingRequestManager Instance { get => _instance; }

        private Queue<PathfindingRequest> pathfindingRequestQueue;
        private bool isProcessingRequest = false;

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

            pathfindingRequestQueue = new Queue<PathfindingRequest>();
        }

        public void RequestPath(
            Vector2 fromPos, Vector2 toPos,
            (List<NodeNeighbourFilter>, List<NodeNeighbourFilter>) filters,
            UnityAction<Pathfinder.PathfindResult, List<Node>> callback)
        {
            pathfindingRequestQueue.Enqueue(new PathfindingRequest(fromPos, toPos, filters, callback));
            ProcessPathfindingRequestQueue();
        }

        private void ProcessPathfindingRequestQueue()
        {
            if (!isProcessingRequest && pathfindingRequestQueue.Count > 0)
            {
                isProcessingRequest = true;
                StartCoroutine(ProcessPathfindingRequest(pathfindingRequestQueue.Dequeue()));
            }
        }

        private IEnumerator ProcessPathfindingRequest(PathfindingRequest request)
        {
            (Pathfinder.PathfindResult result, List<Node> newPath) = Pathfinder.FindPath(
                NodeGrid.Instance,
                request.fromPos,
                request.toPos,
                request.filters);

            request.callback(result, newPath);

            yield return null;

            isProcessingRequest = false;
            ProcessPathfindingRequestQueue();
        }
    }
}
