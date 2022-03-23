using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff : MonoBehaviour
{
    private Coroutine buffTimeout;

    [Header("Combat Changes")]

    [SerializeField] Combat combat;
    [SerializeField] private LayerMask buffedTargetLayer;
    [SerializeField] private float buffDuration;
    private LayerMask defaultTargetLayer;

    [Header("Visual Changes")]

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color buffedColor;
    private Color defaultColor;

    [Space (10)]

    [SerializeField] private PhotonView photonView;

    public bool IsBuffed { get; private set; }

    public void ApplyBuff()
    {
        if (!IsBuffed)
        {
            photonView.RPC("RPC_ApplyBuff", RpcTarget.All);
            buffTimeout = StartCoroutine(ExpireBuff());
        }
    }

    public void RemoveBuff()
    {
        if (IsBuffed)
        {
            photonView.RPC("RPC_RemoveBuff", RpcTarget.All);

            if (buffTimeout != null)
            {
                StopCoroutine(buffTimeout);
                buffTimeout = null;
            }
        }
    }

    private IEnumerator ExpireBuff() 
    {
        yield return new WaitForSeconds(buffDuration);

        RemoveBuff();
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
