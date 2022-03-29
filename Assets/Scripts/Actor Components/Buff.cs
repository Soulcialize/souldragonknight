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
    private float blinkingPeriod = 0.2f;

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
                IsBuffed = false;
                buffTimeout = null;
            }
        }
    }

    private void StartBlinking()
    {
        photonView.RPC("RPC_StartBlinking", RpcTarget.All);
    }

    private IEnumerator ExpireBuff() 
    {
        yield return new WaitForSeconds(Math.Max(0, buffDuration - blinkingDuration));
        if (IsBuffed) StartBlinking();
    }

    private IEnumerator ExpireBlinking()
    {
        int numOfBlinks = (int) Math.Floor(Math.Min(blinkingDuration, buffDuration) / blinkingPeriod);
        int counter = 0;

        while (counter < numOfBlinks) {
            spriteRenderer.color = defaultColor;
            yield return new WaitForSeconds(blinkingPeriod / 2);
            if (!IsBuffed) break;

            spriteRenderer.color = buffedColor;
            yield return new WaitForSeconds(blinkingPeriod / 2);
            if (!IsBuffed) {
                spriteRenderer.color = defaultColor;
                break;
            }

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

    [PunRPC]
    private void RPC_StartBlinking()
    {
        buffTimeout = StartCoroutine(ExpireBlinking());
    }
}
