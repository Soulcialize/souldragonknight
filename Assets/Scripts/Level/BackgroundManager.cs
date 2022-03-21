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
    private float defaultCameraSize;
    private Vector3 defaultBackgroundLocalScale;

    // for background positioning
    private float defaultlocalPosZ;

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

        defaultCameraSize = cam.orthographicSize;
        defaultBackgroundLocalScale = transform.localScale;

        defaultlocalPosZ = transform.localPosition.z;
    }

    private void LateUpdate()
    {
        ScaleBackgroundToCameraSize();
        UpdatePositionRelativeToCamera();
    }

    /// <summary>
    /// Scales the background to match the camera size.
    /// </summary>
    /// <remarks>
    /// This is needed due to the camera's size being different for each player.
    /// </remarks>
    private void ScaleBackgroundToCameraSize()
    {
        transform.localScale = defaultBackgroundLocalScale * cam.orthographicSize / defaultCameraSize;
    }

    /// <summary>
    /// Updates the background's position, taking into account the camera's offset from the center of its boundary.
    /// </summary>
    private void UpdatePositionRelativeToCamera()
    {
        Vector2 cameraOffset = cameraTransform.position - cameraBoundaryTransform.position;

        float cameraHalfWidth = cam.orthographicSize * cam.aspect;
        float horizontalOffsetRatio
            = maxLocalPos.x / (Mathf.Abs(cameraBoundaryTransform.position.x - cameraBoundary.bounds.min.x) - cameraHalfWidth);
        float verticalOffsetRatio
            = maxLocalPos.y / (Mathf.Abs(cameraBoundaryTransform.position.y - cameraBoundary.bounds.min.y) - cam.orthographicSize);

        Vector3 localScale = transform.localScale;
        transform.localPosition = new Vector3(
            -cameraOffset.x * horizontalOffsetRatio * localScale.x,
            -cameraOffset.y * verticalOffsetRatio * localScale.y,
            defaultlocalPosZ);
    }
}
