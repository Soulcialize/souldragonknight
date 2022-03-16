using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonRangedProjectile : RangedProjectile
{
    [SerializeField] LayerMask knightLayer;

    public LayerMask KnightLayer { get; }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (!photonView.IsMine)
        {
            return;
        }

        if (GeneralUtility.IsLayerInLayerMask(collision.gameObject.layer, knightLayer)) {

            EndLifecycle();
        }
    }
}
