using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class HealthUI : MonoBehaviour
{
    [SerializeField] PhotonView photonView;
    [SerializeField] private GameObject[] dragonHealthUI;
    [SerializeField] private GameObject[] knightHealthUI;

    public void UpdateDragonHealthUI(int currHealthPoints)
    {
        photonView.RPC("RPC_UpdateDragonHealthUI", RpcTarget.All, currHealthPoints);
    }

    public void UpdateKnightHealthUI(int currHealthPoints)
    {
        photonView.RPC("RPC_UpdateKnightHealthUI", RpcTarget.All, currHealthPoints);
    }

    [PunRPC]
    private void RPC_UpdateDragonHealthUI(int currHealthPoints)
    {
        for (int i = 0; i < currHealthPoints; i++)
        {
            dragonHealthUI[i].SetActive(true);
        }

        for (int i = currHealthPoints; i < dragonHealthUI.Length; i++)
        {
            dragonHealthUI[i].SetActive(false);
        }
    }

    [PunRPC]
    private void RPC_UpdateKnightHealthUI(int currHealthPoints)
    {
        for (int i = 0; i < currHealthPoints; i++)
        {
            knightHealthUI[i].SetActive(true);
        }

        for (int i = currHealthPoints; i < dragonHealthUI.Length; i++)
        {
            knightHealthUI[i].SetActive(false);
        }
    }
}
