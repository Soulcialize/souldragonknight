using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

using PlayerType = RoleSelectManager.PlayerType;

public class EnemySpawnEvent : UnityEvent<ActorController> { }

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnPoint> spawnPoints;
    [SerializeField] private EnemySpawnEvent enemySpawnEvent;

    public EnemySpawnEvent EnemySpawnEvent { get => enemySpawnEvent; }

    public void SpawnAllEnemies()
    {
        foreach (EnemySpawnPoint spawnPoint in spawnPoints)
        {
            SpawnEnemy(spawnPoint);
        }
    }

    private void SpawnEnemiesForPlayer(PlayerType playerType)
    {
        foreach (EnemySpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.TargetType == playerType)
            {
                SpawnEnemy(spawnPoint);
            }
        }
    }

    private void SpawnEnemy(EnemySpawnPoint spawnPoint)
    {
        ActorController enemy = PhotonNetwork.Instantiate(
            spawnPoint.PrefabToSpawn.name,
            spawnPoint.transform.position,
            spawnPoint.transform.rotation).GetComponent<ActorController>();

        enemySpawnEvent.Invoke(enemy);
    }
}
