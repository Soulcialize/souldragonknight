using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class LevelSectionManager : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;

    [Space(10)]

    [SerializeField] private UnityEvent levelClearedEvent;

    private int numEnemiesSpawned = 0;
    private int numEnemiesKilled = 0;

    public void EnemySpawnHandler(ActorController enemy)
    {
        photonView.RPC("RPC_EnemySpawnHandler", RpcTarget.All);
        enemy.Combat.DeathEvent.AddListener(EnemyDeathHandler);
    }

    [PunRPC]
    private void RPC_EnemySpawnHandler()
    {
        numEnemiesSpawned++;
    }

    private void EnemyDeathHandler()
    {
        photonView.RPC("RPC_EnemyDeathHandler", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_EnemyDeathHandler()
    {
        numEnemiesKilled++;
        if (photonView.IsMine && numEnemiesKilled >= numEnemiesSpawned)
        {
            photonView.RPC("RPC_SignalLevelCleared", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_SignalLevelCleared()
    {
        levelClearedEvent.Invoke();
    }
}
