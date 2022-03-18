using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using PlayerType = RoleSelectManager.PlayerType;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnPoint> spawnPoints;

    public void SpawnAllEnemies()
    {
        foreach (EnemySpawnPoint spawnPoint in spawnPoints)
        {
            SpawnEnemy(spawnPoint);
        }
    }

    public void SpawnEnemiesForPlayer(PlayerType playerType)
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
        PhotonNetwork.Instantiate(
            spawnPoint.PrefabToSpawn.name,
            spawnPoint.transform.position,
            spawnPoint.transform.rotation);
    }
}
