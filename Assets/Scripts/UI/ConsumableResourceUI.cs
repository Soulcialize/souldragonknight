using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public void RegenerateStaminaUI()
    {
        photonView.RPC("RPC_RegenerateStaminaUI", RpcTarget.All);
    }

    public void RegenerateManaUI()
    {
        photonView.RPC("RPC_RegenerateManaUI", RpcTarget.All);
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
    private void RPC_RegenerateStaminaUI()
    {
        staminaBar.StartRegeneration();
    }

    [PunRPC]
    private void RPC_RegenerateManaUI()
    {
        manaBar.StartRegeneration();
    }
}
