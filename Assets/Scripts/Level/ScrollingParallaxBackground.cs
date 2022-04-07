using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingParallaxBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float parallaxYMultiplier;

    private Camera cam;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    // for background scaling
    private float defaultCameraSize;
    private Vector3 defaultBackgroundLocalScale;

    // for infinite scrolling
    private float textureUnitSizeX;

    private void Awake()
    {
        cam = Camera.main;
        cameraTransform = cam.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        textureUnitSizeX = sprite.texture.width / sprite.pixelsPerUnit;

        defaultCameraSize = cam.orthographicSize;
        defaultBackgroundLocalScale = transform.localScale;
    }

    private void LateUpdate()
    {
        ScaleBackgroundToCameraSize();

        // scroll on horizontal axis; parallax on vertical axis
        transform.position += new Vector3(
            scrollSpeed * Time.deltaTime,
            (cameraTransform.position - lastCameraPosition).y * parallaxYMultiplier);

        lastCameraPosition = cameraTransform.position;

        // shift background so it can infinitely scroll
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPosX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPosX, transform.position.y);
        }
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
}
