using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff : MonoBehaviour
{
    [SerializeField] Combat combat;
    [SerializeField] private LayerMask buffedTargetLayer;
    [SerializeField] private LayerMask mainTargetLayer;

    [SerializeField] private PhotonView photonView;

    public bool IsBuffed { get; set; }

    public void ApplyBuff()
    {
        photonView.RPC("RPC_ApplyBuff", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_ApplyBuff()
    {
        combat.AttackEffectLayer = buffedTargetLayer;
        IsBuffed = true;
    }

    public void RemoveBuff()
    {
        combat.AttackEffectLayer = mainTargetLayer;
        IsBuffed = false;
    }
}
