using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff : MonoBehaviour
{
    [Header("Combat Changes")]

    [SerializeField] Combat combat;
    [SerializeField] private LayerMask buffedTargetLayer;
    private LayerMask defaultTargetLayer;

    [Header("Visual Changes")]

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color buffedColor;
    private Color defaultColor;

    [Space (10)]

    [SerializeField] private PhotonView photonView;

    public bool IsBuffed { get; set; }

    public void ApplyBuff()
    {
        photonView.RPC("RPC_ApplyBuff", RpcTarget.All);
    }

    public void RemoveBuff()
    {
        photonView.RPC("RPC_RemoveBuff", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_ApplyBuff()
    {
        defaultTargetLayer = combat.AttackEffectLayer;
        defaultColor = spriteRenderer.color;

        combat.AttackEffectLayer = buffedTargetLayer;
        spriteRenderer.color = buffedColor;
        IsBuffed = true;
    }

    [PunRPC]
    private void RPC_RemoveBuff()
    {
        combat.AttackEffectLayer = defaultTargetLayer;
        spriteRenderer.color = defaultColor;
        IsBuffed = false;
    }
}
