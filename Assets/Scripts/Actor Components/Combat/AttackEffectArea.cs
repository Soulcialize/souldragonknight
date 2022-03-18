using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackEffectArea : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private bool drawAreas = true;

    public Vector2 Size { get => size; }

    public Vector2 TopCornerPos { get; private set; }

    private void Awake()
    {
        TopCornerPos = (Vector2)transform.localPosition + new Vector2(size.x / 2f, size.y / 2f);
    }

    private void OnDrawGizmos()
    {
        if (drawAreas)
        {
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}
