using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject dragonPrefab;

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount % 2 == 0)
        {
            PhotonNetwork.Instantiate(dragonPrefab.name, new Vector2(-6f, 2f), dragonPrefab.transform.rotation);
        }
        else
        {
            PhotonNetwork.Instantiate(knightPrefab.name, new Vector2(-7f, -0.5f), knightPrefab.transform.rotation);
        }
    }
}
