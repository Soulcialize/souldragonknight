using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class NodeGridUpdater : MonoBehaviour
    {
        private static NodeGridUpdater _instance;
        public static NodeGridUpdater Instance { get => _instance; }

        private Queue<(Vector2 updateRegionMin, Vector2 updateRegionMax)> gridUpdateRequestQueue;
        private bool isUpdatingGrid = false;

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

            gridUpdateRequestQueue = new Queue<(Vector2 updateRegionMin, Vector2 updateRegionMax)>();
        }

        public void RequestGridUpdate(Vector2 regionMinPoint, Vector2 regionMaxPoint)
        {
            gridUpdateRequestQueue.Enqueue((regionMinPoint, regionMaxPoint));
            ProcessGridUpdateQueue();
        }

        private void ProcessGridUpdateQueue()
        {
            if (!isUpdatingGrid && gridUpdateRequestQueue.Count > 0)
            {
                isUpdatingGrid = true;
                (Vector2 regionMinPoint, Vector2 regionMaxPoint) = gridUpdateRequestQueue.Dequeue();
                StartCoroutine(UpdateGridRegion(regionMinPoint, regionMaxPoint));
            }
        }

        private IEnumerator UpdateGridRegion(Vector2 regionMinPoint, Vector2 regionMaxPoint)
        {
            NodeGrid.Instance.UpdateGridRegion(regionMinPoint, regionMaxPoint);

            yield return null;

            isUpdatingGrid = false;
            ProcessGridUpdateQueue();
        }
    }
}
