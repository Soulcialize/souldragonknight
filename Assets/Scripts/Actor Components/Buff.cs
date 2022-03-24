using System;
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

    private float blinkingDuration = 1.0f;
    private float blinkingPeriod = 0.17f;

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

     private void ApplyBuffColor()
    {
        if (IsBuffed)
        {
            photonView.RPC("RPC_ApplyBuffColor", RpcTarget.All);
        }
    }

    private void RemoveBuffColor()
    {
        if (IsBuffed)
        {
            photonView.RPC("RPC_RemoveBuffColor", RpcTarget.All);
        }
    }

    private IEnumerator ExpireBuff() 
    {
        yield return new WaitForSeconds(Math.Max(0, buffDuration - blinkingDuration));

        int numOfBlinks = (int) Math.Floor(Math.Min(blinkingDuration, buffDuration) / blinkingPeriod);
        int counter = 0;

        while (counter < numOfBlinks) {
            RemoveBuffColor();
            yield return new WaitForSeconds(blinkingPeriod / 2);
            ApplyBuffColor();
            yield return new WaitForSeconds(blinkingPeriod / 2);
            counter++;
        }

        RemoveBuff();
    }

    [PunRPC]
    private void RPC_ApplyBuff()
    {
        defaultTargetLayer = combat.AttackEffectLayer;
        defaultColor = spriteRenderer.color;

        combat.AttackEffectLayer = buffedTargetLayer;
        RPC_ApplyBuffColor();
        IsBuffed = true;
    }

    [PunRPC]
    private void RPC_RemoveBuff()
    {
        combat.AttackEffectLayer = defaultTargetLayer;
        RPC_RemoveBuffColor();
        IsBuffed = false;
    }

    [PunRPC]
    private void RPC_ApplyBuffColor()
    {
        if (defaultColor != null) {
            spriteRenderer.color = buffedColor;
        }
    }

    [PunRPC]
    private void RPC_RemoveBuffColor()
    {
        if (defaultColor != null) {
            spriteRenderer.color = defaultColor;
        }
    }
}
