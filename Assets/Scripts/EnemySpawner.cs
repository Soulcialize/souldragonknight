using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner instance;
    public static EnemySpawner Instance { get => instance; }

    [SerializeField] private GameObject knightEnemyPrefab;
    [SerializeField] private Transform knightEnemySpawnPoint;

    [Space(10)]

    [SerializeField] private GameObject dragonEnemyPrefab;
    [SerializeField] private Transform dragonEnemySpawnPoint;

    [Space(10)]

    [SerializeField] private float minTimeBetweenSpawns;
    [SerializeField] private float maxTimeBetweenSpawns;

    private EnemyController.Type typeToSpawn;
    private float timeOfNextSpawn;

    private void Awake()
    {
        // singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.NickName == "Knight")
        {
            typeToSpawn = EnemyController.Type.KNIGHT;
        }
        else
        {
            typeToSpawn = EnemyController.Type.DRAGON;
        }

        timeOfNextSpawn = Time.time + Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2 || Time.time < timeOfNextSpawn)
        {
            return;
        }

        timeOfNextSpawn = Time.time + Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        Spawn(typeToSpawn);
    }

    public GameObject Spawn(EnemyController.Type type)
    {
        GameObject enemyObj = null;
        if (type == EnemyController.Type.KNIGHT)
        {
            enemyObj = PhotonNetwork.Instantiate(knightEnemyPrefab.name, knightEnemySpawnPoint.position, Quaternion.identity);
        }
        else if (type == EnemyController.Type.DRAGON)
        {
            enemyObj = PhotonNetwork.Instantiate(dragonEnemyPrefab.name, dragonEnemySpawnPoint.position, Quaternion.identity);
        }

        return enemyObj;
    }
}
