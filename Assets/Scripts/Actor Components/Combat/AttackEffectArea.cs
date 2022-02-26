using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackEffectArea : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    public Vector2 Size { get => size; }

    [SerializeField] private bool drawAreas = true;

    private void OnDrawGizmos()
    {
        if (drawAreas)
        {
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}
