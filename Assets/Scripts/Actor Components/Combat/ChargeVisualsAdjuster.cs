using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChargeVisualsAdjuster : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ChargeCombat combat;

    [Space(10)]

    [SerializeField] private Vector3 normalSize;
    [SerializeField] private Vector3 chargeSize;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color chargeColor;

    [Space(10)]

    [Tooltip("Time taken to deflate back to normal if inflation is interrupted.")]
    [SerializeField] private float interruptDeflationTime;

    private Coroutine adjustVisualsCoroutine;

    private void StopRunningCoroutine()
    {
        if (adjustVisualsCoroutine != null)
        {
            StopCoroutine(adjustVisualsCoroutine);
        }
    }

    public void InflateToCharge()
    {
        StopRunningCoroutine();
        adjustVisualsCoroutine = StartCoroutine(
            AdjustVisualsForCharge(normalColor, chargeColor, normalSize, chargeSize, combat.LockTargetPositionTime));
    }

    public void DeflateFromCharge()
    {
        StopRunningCoroutine();
        adjustVisualsCoroutine = StartCoroutine(
            AdjustVisualsForCharge(chargeColor, normalColor, chargeSize, normalSize, combat.ChargeRecoveryTime));
    }

    public void InterruptInflation()
    {
        StopRunningCoroutine();
        
        // we need to grab the absolute value of the scale's x value (i.e. ignore direction the actor is facing)
        Vector3 startSize = transform.localScale;
        startSize.x = Mathf.Abs(startSize.x);

        adjustVisualsCoroutine = StartCoroutine(
            AdjustVisualsForCharge(spriteRenderer.color, normalColor, startSize, normalSize, interruptDeflationTime));
    }

    private IEnumerator AdjustVisualsForCharge(
        Color initialColor, Color targetColor, Vector3 initialScale, Vector3 targetScale, float duration)
    {
        float timePassed = 0f;
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;

            Color tempColor = Color.Lerp(initialColor, targetColor, timePassed / duration);
            tempColor.a = spriteRenderer.color.a;
            spriteRenderer.color = tempColor;
            photonView.RPC("RPC_AdjustColor", RpcTarget.Others, tempColor.r, tempColor.g, tempColor.b);

            Vector3 tempScale = Vector3.Lerp(initialScale, targetScale, timePassed / duration);
            if (Mathf.Sign(tempScale.x) != Mathf.Sign(transform.localScale.x))
            {
                tempScale.x = -tempScale.x;
            }

            transform.localScale = tempScale;

            yield return null;
        }

        targetColor.a = spriteRenderer.color.a;
        photonView.RPC("RPC_AdjustColor", RpcTarget.Others, targetColor.r, targetColor.g, targetColor.b);
        spriteRenderer.color = targetColor;

        if (Mathf.Sign(targetScale.x) != Mathf.Sign(transform.localScale.x))
        {
            targetScale.x = -targetScale.x;
        }

        transform.localScale = targetScale;
    }

    [PunRPC]
    private void RPC_AdjustColor(float r, float g, float b)
    {
        spriteRenderer.color = new Color(r, g, b, spriteRenderer.color.a);
    }
}
