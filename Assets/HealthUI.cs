using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class HealthUI : MonoBehaviour
{
    [SerializeField] private GameObject[] dragonHealthUI;
    [SerializeField] private GameObject[] knightHealthUI;
    [SerializeField] private int dragonHealth;
    [SerializeField] private int knightHealth;
    [SerializeField] PhotonView photonView;

    public int DragonHealth { get => dragonHealth; }
    public int KnightHealth { get => knightHealth; }
    // Start is called before the first frame update

    public void DecrementDragonHealthUI()
    {
        photonView.RPC("RPC_DecrementDragonHealthUI", RpcTarget.All);
    }

    public void DecrementKnightHealthUI()
    {
        photonView.RPC("RPC_DecrementKnightHealthUI", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_DecrementDragonHealthUI()
    {
        dragonHealth = Mathf.Max(0, dragonHealth - 1);
        dragonHealthUI[dragonHealth].SetActive(false);
    }

    [PunRPC]
    private void RPC_DecrementKnightHealthUI()
    {
        knightHealth = Mathf.Max(0, knightHealth - 1);
        knightHealthUI[knightHealth].SetActive(false);
    }
}
