using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayerType = RoleSelectManager.PlayerType;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private PlayerType targetType;
    [SerializeField] private ActorController prefabToSpawn;

    public PlayerType TargetType { get => targetType; }
    public ActorController PrefabToSpawn { get => prefabToSpawn; }
}
