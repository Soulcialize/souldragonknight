using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CollisionLayer : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Collider2D collider2d;

    private int originalLayer;

    private void Awake()
    {
        originalLayer = collider2d.gameObject.layer;
    }

    public void SetLayer(int layer)
    {
        photonView.RPC("RPC_SetCollisionLayer", RpcTarget.All, layer);
    }

    public void ResetLayer()
    {
        photonView.RPC("RPC_SetCollisionLayer", RpcTarget.All, originalLayer);
    }

    [PunRPC]
    private void RPC_SetCollisionLayer(int layer)
    {
        collider2d.gameObject.layer = layer;
    }
}
