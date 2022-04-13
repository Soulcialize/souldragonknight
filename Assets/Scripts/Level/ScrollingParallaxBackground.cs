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

    // for infinite scrolling
    private float textureUnitSizeX;

    private void Awake()
    {
        cam = Camera.main;
        cameraTransform = cam.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        textureUnitSizeX = sprite.texture.width / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
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
}
