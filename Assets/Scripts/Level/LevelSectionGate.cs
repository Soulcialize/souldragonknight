using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSectionGate : MonoBehaviour
{
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

        while (elapsedTime < timeTaken)
        {
            gateTransform.position = Vector2.Lerp(startPos, finalPos, elapsedTime / timeTaken);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gateTransform.position = finalPos;
    }
}
