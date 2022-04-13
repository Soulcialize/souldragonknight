using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConsumableResourceUI : MonoBehaviour
{
    [SerializeField] private ResourceBar resourceBar;
    [SerializeField] private PhotonView photonView;

    public void UpdateAmount(float currAmount)
    {
        photonView.RPC("RPC_UpdateAmount", RpcTarget.All, currAmount);
    }

    public void Regenerate(float regenSpeed)
    {
        photonView.RPC("RPC_Regenerate", RpcTarget.All, regenSpeed);
    }

    public void StopRegenerate()
    {
        photonView.RPC("RPC_StopRegenerate", RpcTarget.All);
    }

    public void FlashWarning()
    {
        resourceBar.FlashWarningIfNotRunning();
    }

    [PunRPC]
    private void RPC_UpdateAmount(float currAmount)
    {
        resourceBar.UpdateValue(currAmount);
    }

    [PunRPC]
    private void RPC_Regenerate(float regenSpeed)
    {
        resourceBar.StartRegeneration(regenSpeed);
    }

    [PunRPC]
    private void RPC_StopRegenerate()
    {
        resourceBar.StopRegeneration();
    }
}
