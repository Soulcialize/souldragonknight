using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"spawning player");
        PhotonNetwork.Instantiate(
            playerPrefab.name,
            playerPrefab.transform.position + new Vector3(Random.Range(-1f, 1f), playerPrefab.transform.position.y, playerPrefab.transform.position.z),
            playerPrefab.transform.rotation);
    }
}
