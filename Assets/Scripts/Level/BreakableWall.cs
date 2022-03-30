using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Collider2D collider2d;

    public void HandleHit()
    {
        photonView.RPC("RPC_HandleHit", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_HandleHit()
    {
        // get collider bounds before disabling
        Vector2 updateRegionMinPoint = collider2d.bounds.min;
        Vector2 updateRegionMaxPoint = collider2d.bounds.max;

        collider2d.enabled = false;
        Pathfinding.NodeGrid.Instance.RequestGridUpdate(updateRegionMinPoint, updateRegionMaxPoint);

        PhotonNetwork.Destroy(gameObject);
    }
}
