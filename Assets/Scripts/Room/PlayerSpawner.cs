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
    [SerializeField] private float knightCameraSize;
    [SerializeField] private float dragonCameraSize;

    public static PlayerType GetLocalPlayerType()
    {
        return (PlayerType)PhotonNetwork.LocalPlayer.CustomProperties[PLAYER_PROPERTIES_TYPE_KEY];
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnKnight()
    {
        var knightObj = PhotonNetwork.Instantiate(knightPrefab.name, knightSpawnPoint.position, knightPrefab.transform.rotation);
        CVCamera.m_Follow = knightObj.transform;
        CVCamera.m_Lens.OrthographicSize = knightCameraSize;
    }

    private void SpawnDragon()
    {
        var dragonObj = PhotonNetwork.Instantiate(dragonPrefab.name, dragonSpawnPoint.position, dragonPrefab.transform.rotation);
        CVCamera.m_Follow = dragonObj.transform;
        CVCamera.m_Lens.OrthographicSize = dragonCameraSize;
    }
    
    private void SpawnPlayer()
    {
        SpawnPlayer(GetLocalPlayerType());
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
