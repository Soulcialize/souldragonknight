using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Buff : MonoBehaviour
{
    private readonly float timerDuration = 3f;

    private float timer;
    private bool isTimerRunning = false;

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

    private void Update()
    {
        if (isTimerRunning)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else 
            {
                RemoveBuff();
                isTimerRunning = false;
            }
        }
    }

    public void ApplyBuff()
    {
        photonView.RPC("RPC_ApplyBuff", RpcTarget.All);
    }

    public void RemoveBuff()
    {
        photonView.RPC("RPC_RemoveBuff", RpcTarget.All);
    }

    private void StartTimer()
    {
        isTimerRunning = true;
        timer = timerDuration;
    }

    private void StopTimer()
    {
        isTimerRunning = false;
        timer = 0f;
    }

    [PunRPC]
    private void RPC_ApplyBuff()
    {
        defaultTargetLayer = combat.AttackEffectLayer;
        defaultColor = spriteRenderer.color;

        combat.AttackEffectLayer = buffedTargetLayer;
        spriteRenderer.color = buffedColor;
        IsBuffed = true;

        StartTimer();
    }

    [PunRPC]
    private void RPC_RemoveBuff()
    {
        combat.AttackEffectLayer = defaultTargetLayer;
        spriteRenderer.color = defaultColor;
        IsBuffed = false;

        StopTimer();
    }
}
