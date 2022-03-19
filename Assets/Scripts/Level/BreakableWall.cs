using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;

    public void HandleHit()
    {
        photonView.RPC("RPC_HandleHit", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_HandleHit()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
