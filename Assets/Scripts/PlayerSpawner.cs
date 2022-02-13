using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab.name, Vector2.zero, playerPrefab.transform.rotation);
    }
}
