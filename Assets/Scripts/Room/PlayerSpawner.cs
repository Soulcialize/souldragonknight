using Photon.Pun;
using UnityEngine;
using PlayerType = RoleSelectManager.PlayerType;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    public static readonly string PLAYER_PROPERTIES_TYPE_KEY = "PlayerType";

    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private Transform knightSpawnPoint;

    [Space(10)]

    [SerializeField] private GameObject dragonPrefab;
    [SerializeField] private Transform dragonSpawnPoint;

    [Space(10)]

    [SerializeField] private CinemachineVirtualCamera CVCamera;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnKnight()
    {
        var knightObj = PhotonNetwork.Instantiate(knightPrefab.name, knightSpawnPoint.position, knightPrefab.transform.rotation);
        CVCamera.m_Follow = knightObj.transform;
        
        // TODO: consider doing this stuff in other scripts
        // BackgroundManager.Instance.ActivateSoulWorldBackground();
    }

    private void SpawnDragon()
    {
        var dragonObj = PhotonNetwork.Instantiate(dragonPrefab.name, dragonSpawnPoint.position, dragonPrefab.transform.rotation);
        CVCamera.m_Follow = dragonObj.transform;

        // TODO: consider doing this stuff in other scripts
        // BackgroundManager.Instance.ActivateRealWorldBackground();
    }
    
    private void SpawnPlayer()
    {
        SpawnPlayer((PlayerType)PhotonNetwork.LocalPlayer.CustomProperties[PLAYER_PROPERTIES_TYPE_KEY]);
    }

    private void SpawnPlayer(PlayerType playerType)
    {
        switch (playerType)
        {
            case PlayerType.KNIGHT:
                SpawnKnight();
                break;
            case PlayerType.DRAGON:
                SpawnDragon();
                break;
            default:
                throw new System.ArgumentException($"Player of type " +
                    $"{System.Enum.GetName(typeof(PlayerType), playerType)} cannot be spawned");
        }
    }
}
