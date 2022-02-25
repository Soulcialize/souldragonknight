using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackEffectArea : MonoBehaviour
{
    [SerializeField] private Transform leftOrigin;
    public Transform LeftOrigin { get => leftOrigin; }

    [SerializeField] private Transform rightOrigin;
    public Transform RightOrigin { get => rightOrigin; }

    [SerializeField] private Vector2 size;
    public Vector2 Size { get => size; }

    [SerializeField] private bool drawAreas = true;

    private void OnDrawGizmos()
    {
        if (drawAreas)
        {
            Gizmos.DrawWireCube(leftOrigin.position, size);
            Gizmos.DrawWireCube(rightOrigin.position, size);
        }
    }
}
