using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform knightSpawnPoint;
    [SerializeField] private Transform dragonSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        Transform spawnPoint = GameManager.Instance.CurrPlayerType == GameManager.PlayerType.KNIGHT ? knightSpawnPoint : dragonSpawnPoint;
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, playerPrefab.transform.rotation);
    }
}
