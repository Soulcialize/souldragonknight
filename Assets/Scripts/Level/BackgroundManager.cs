using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance { get; private set; }

    [Header("Background Scaling")]

    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Background Position")]

    [SerializeField] private PolygonCollider2D cameraBoundary;
    [SerializeField] private Vector2 minLocalPos;
    [SerializeField] private Vector2 maxLocalPos;

    private Camera cam;
    private Transform cameraTransform;
    private Transform cameraBoundaryTransform;

    // for background scaling
    private Vector3 defaultBackgroundLocalScale;
    private float backgroundPixelsPerUnitPercent;

    // for background positioning
    private float horizontalOffsetRatio;
    private float verticalOffsetRatio;
    private float localPosZ;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // initialize variables
        cam = Camera.main;
        cameraTransform = cam.transform;
        cameraBoundaryTransform = cameraBoundary.transform;

        horizontalOffsetRatio
            = Mathf.Abs(maxLocalPos.x - minLocalPos.x) / Mathf.Abs(cameraBoundaryTransform.position.x - cameraBoundary.bounds.min.x);
        verticalOffsetRatio
            = Mathf.Abs(maxLocalPos.y - minLocalPos.y) / Mathf.Abs(cameraBoundaryTransform.position.y - cameraBoundary.bounds.min.y);
        localPosZ = transform.localPosition.z;

        defaultBackgroundLocalScale = transform.localScale;
        backgroundPixelsPerUnitPercent = spriteRenderer.sprite.pixelsPerUnit / 100f;
    }

    private void LateUpdate()
    {
        UpdatePositionRelativeToCamera();
        ScaleBackgroundToCameraSize();
    }

    /// <summary>
    /// Scales the background to match the camera size.
    /// </summary>
    /// <remarks>
    /// This is mainly needed due to the camera's size being dependent on the player's screen size.
    /// </remarks>
    private void ScaleBackgroundToCameraSize()
    {
        float cameraHeight = cam.orthographicSize * 2f;
        Vector2 cameraSize = new Vector2(cam.aspect * cameraHeight, cameraHeight);
        Vector2 backgroundSize = spriteRenderer.sprite.bounds.size * backgroundPixelsPerUnitPercent;

        // scale background based on aspect ratio's larger dimension
        Vector2 backgroundScale = defaultBackgroundLocalScale;
        backgroundScale *= cameraSize.x >= cameraSize.y
            ? cameraSize.x / backgroundSize.x // landscape mode
            : cameraSize.y / backgroundSize.y; // portrait mode

        transform.localScale = backgroundScale;
    }

    /// <summary>
    /// Updates the background's position, taking into account the camera's offset from the center of its boundary.
    /// </summary>
    private void UpdatePositionRelativeToCamera()
    {
        Vector2 cameraOffset = cameraTransform.position - cameraBoundaryTransform.position;
        transform.localPosition = new Vector3(
            -cameraOffset.x * horizontalOffsetRatio,
            -cameraOffset.y * verticalOffsetRatio,
            localPosZ);
    }
}
