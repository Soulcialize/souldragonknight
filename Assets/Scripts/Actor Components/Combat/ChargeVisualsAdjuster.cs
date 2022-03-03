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

    private Coroutine adjustVisualsCoroutine;

    public void InflateToCharge()
    {
        if (adjustVisualsCoroutine != null)
        {
            StopCoroutine(adjustVisualsCoroutine);
        }

        adjustVisualsCoroutine = StartCoroutine(
            AdjustVisualsForCharge(normalColor, chargeColor, normalSize, chargeSize, combat.LockTargetPositionTime));
    }

    public void DeflateFromCharge()
    {
        if (adjustVisualsCoroutine != null)
        {
            StopCoroutine(adjustVisualsCoroutine);
        }

        adjustVisualsCoroutine = StartCoroutine(
            AdjustVisualsForCharge(chargeColor, normalColor, chargeSize, normalSize, combat.ChargeRecoveryTime));
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
            photonView.RPC("RPC_AdjustColor", RpcTarget.Others, tempColor.r, tempColor.g, tempColor.b, tempColor.a);

            Vector3 tempScale = Vector3.Lerp(initialScale, targetScale, timePassed / duration);
            tempScale.x *= Mathf.Sign(transform.localScale.x);
            transform.localScale = tempScale;

            yield return null;
        }

        targetColor.a = spriteRenderer.color.a;
        photonView.RPC("RPC_AdjustColor", RpcTarget.Others, targetColor.r, targetColor.g, targetColor.b, targetColor.a);
        spriteRenderer.color = targetColor;

        targetScale.x *= Mathf.Sign(transform.localScale.x);
        transform.localScale = targetScale;
    }

    [PunRPC]
    private void RPC_AdjustColor(float r, float g, float b, float a)
    {
        spriteRenderer.color = new Color(r, g, b, a);
    }
}
