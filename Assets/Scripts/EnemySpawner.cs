using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private GameManager.PlayerType targetType;
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
        targetType = GameManager.Instance.CurrPlayerType;
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
        Spawn(targetType);
    }

    public GameObject Spawn(GameManager.PlayerType targetType)
    {
        GameObject enemyObj = null;
        if (targetType == GameManager.PlayerType.KNIGHT)
        {
            enemyObj = Instantiate(knightEnemyPrefab, knightEnemySpawnPoint.position, Quaternion.identity);
        }
        else if (targetType == GameManager.PlayerType.DRAGON)
        {
            enemyObj = Instantiate(dragonEnemyPrefab, dragonEnemySpawnPoint.position, Quaternion.identity);
        }

        return enemyObj;
    }
}
