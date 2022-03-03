using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceDetector : MonoBehaviour
{
    [SerializeField] private LayerMask surfacesLayerMask;

    private int numCollidersInContact = 0;

    public bool IsInContact { get => numCollidersInContact > 0; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GeneralUtility.IsLayerInLayerMask(other.gameObject.layer, surfacesLayerMask))
        {
            numCollidersInContact++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (GeneralUtility.IsLayerInLayerMask(other.gameObject.layer, surfacesLayerMask))
        {
            numCollidersInContact--;
        }
    }
}
