using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject knightEnemyPrefab;
    [SerializeField] private Transform knightEnemySpawnPoint;

    [Space(10)]

    [SerializeField] private GameObject dragonEnemyPrefab;
    [SerializeField] private Transform dragonEnemySpawnPoint;

    [Space(10)]

    [SerializeField] private float minTimeBetweenSpawns;
    [SerializeField] private float maxTimeBetweenSpawns;

    private GameObject enemyPrefab;
    private Vector2 spawnPoint;

    private float timeOfNextSpawn;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.NickName == "Knight")
        {
            enemyPrefab = knightEnemyPrefab;
            spawnPoint = knightEnemySpawnPoint.position;
        }
        else
        {
            enemyPrefab = dragonEnemyPrefab;
            spawnPoint = dragonEnemySpawnPoint.position;
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
        PhotonNetwork.Instantiate(enemyPrefab.name, spawnPoint, Quaternion.identity);
    }
}
