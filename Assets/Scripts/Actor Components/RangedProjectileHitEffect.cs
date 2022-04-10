using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectileHitEffect : MonoBehaviour
{
    public void EndLifecycle()
    {
        Destroy(gameObject);
    }
}
