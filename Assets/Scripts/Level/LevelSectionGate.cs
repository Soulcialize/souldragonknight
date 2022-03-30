using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSectionGate : MonoBehaviour
{
    [SerializeField] private Collider2D collider2d;
    [SerializeField] private float raisedHeight;
    [SerializeField] private float speed;

    private Transform gateTransform;
    private Coroutine openGateCoroutine;

    private void Awake()
    {
        gateTransform = transform;
    }

    public void Open()
    {
        if (openGateCoroutine == null)
        {
            openGateCoroutine = StartCoroutine(OpenGate());
        }
    }

    private IEnumerator OpenGate()
    {
        Vector2 startPos = gateTransform.position;
        Vector2 finalPos = (Vector2)gateTransform.position + Vector2.up * raisedHeight;

        float timeTaken = raisedHeight / speed;
        float elapsedTime = 0f;

        StartCoroutine(UpdatePathfindingGrid(timeTaken));
        while (elapsedTime < timeTaken)
        {
            gateTransform.position = Vector2.Lerp(startPos, finalPos, elapsedTime / timeTaken);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gateTransform.position = finalPos;
    }

    private IEnumerator UpdatePathfindingGrid(float duration)
    {
        Vector2 updateRegionMinPoint = collider2d.bounds.min;
        Vector2 updateRegionMaxPoint = collider2d.bounds.max + Vector3.up * raisedHeight;
        float updateInterval = Pathfinding.NodeGrid.MIN_GRID_UPDATE_INTERVAL;
        for (float i = 0; i < duration; i += updateInterval)
        {
            yield return new WaitForSeconds(0.5f);
            Pathfinding.NodeGrid.Instance.UpdateGridRegion(updateRegionMinPoint, updateRegionMaxPoint);
        }
    }
}
