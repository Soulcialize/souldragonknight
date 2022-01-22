using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [Space(10)]

    [SerializeField] private Transform knightEnemySpawnPoint;
    [SerializeField] private LayerMask knightLayerMask;

    [Space(10)]

    [SerializeField] private Transform dragonEnemySpawnPoint;
    [SerializeField] private LayerMask dragonLayerMask;

    [Space(10)]

    [SerializeField] private float minTimeBetweenSpawns;
    [SerializeField] private float maxTimeBetweenSpawns;

    private Vector2 spawnPoint;
    private LayerMask playerLayerMask;

    private float timeOfNextSpawn;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.NickName == "Knight")
        {
            spawnPoint = knightEnemySpawnPoint.position;
            playerLayerMask = knightLayerMask;
        }
        else
        {
            spawnPoint = dragonEnemySpawnPoint.position;
            playerLayerMask = dragonLayerMask;
        }

        timeOfNextSpawn = Time.time + Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < timeOfNextSpawn)
        {
            return;
        }

        timeOfNextSpawn = Time.time + Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        GameObject enemyObj = PhotonNetwork.Instantiate(enemyPrefab.name, spawnPoint, Quaternion.identity);
        enemyObj.GetComponentInChildren<EnemyController>().SetPlayerLayerMask(playerLayerMask);
    }
}
