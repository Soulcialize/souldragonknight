using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackEffectArea : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private bool drawAreas = true;

    public Vector2 Size { get => size; }

    public Vector2 LocalPos { get; private set; }

    /// <summary>
    /// The local position of the area's bottom left corner while facing right.
    /// </summary>
    public Vector2 MinLocalPos { get; private set; }

    /// <summary>
    /// The local position of the area's top right corner while facing right.
    /// </summary>
    public Vector2 MaxLocalPos { get; private set; }

    private void Awake()
    {
        Vector2 extents = new Vector2(size.x / 2f, size.y / 2f);
        LocalPos = transform.localPosition;
        MinLocalPos = (Vector2)transform.localPosition - extents;
        MaxLocalPos = (Vector2)transform.localPosition + extents;
    }

    private void OnDrawGizmos()
    {
        if (drawAreas)
        {
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}
