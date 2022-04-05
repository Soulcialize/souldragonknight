using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConsumableResourceUI : MonoBehaviour
{
    [SerializeField] private ResourceBar staminaBar;
    [SerializeField] private ResourceBar manaBar;
    [SerializeField] private PhotonView photonView;

    public void UpdateStaminaUI(float currAmount)
    {
        photonView.RPC("RPC_UpdateStaminaUI", RpcTarget.All, currAmount);
    }

    public void UpdateManaUI(float currAmount)
    {
        photonView.RPC("RPC_UpdateManaUI", RpcTarget.All, currAmount);
    }

    public void RegenerateStaminaUI(float regenSpeed)
    {
        photonView.RPC("RPC_RegenerateStaminaUI", RpcTarget.All, regenSpeed);
    }

    public void RegenerateManaUI(float regenSpeed)
    {
        photonView.RPC("RPC_RegenerateManaUI", RpcTarget.All, regenSpeed);
    }

    public void StopRegenStaminaUI()
    {
        photonView.RPC("RPC_StopRegenStaminaUI", RpcTarget.All);
    }

    public void StopRegenManaUI()
    {
        photonView.RPC("RPC_StopRegenManaUI", RpcTarget.All);
    }

    public void FlashStaminaWarning()
    {
        staminaBar.FlashWarningIfNotRunning();
    }

    public void FlashManaWarning()
    {
        manaBar.FlashWarningIfNotRunning();
    }

    [PunRPC]
    private void RPC_UpdateStaminaUI(float currAmount)
    {
        staminaBar.UpdateValue(currAmount);
    }

    [PunRPC]
    private void RPC_UpdateManaUI(float currAmount)
    {
        manaBar.UpdateValue(currAmount);
    }

    [PunRPC]
    private void RPC_RegenerateStaminaUI(float regenSpeed)
    {
        staminaBar.StartRegeneration(regenSpeed);
    }

    [PunRPC]
    private void RPC_RegenerateManaUI(float regenSpeed)
    {
        manaBar.StartRegeneration(regenSpeed);
    }

    [PunRPC]
    private void RPC_StopRegenStaminaUI()
    {
        staminaBar.StopRegeneration();
    }

    [PunRPC]
    private void RPC_StopRegenManaUI()
    {
        manaBar.StopRegeneration();
    }
}
