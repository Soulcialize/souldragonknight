using Photon.Pun;
using UnityEngine;
using PlayerType = RoleSelectManager.PlayerType;

public class PlayerSpawner : MonoBehaviour
{
    public static readonly string PLAYER_PROPERTIES_TYPE_KEY = "PlayerType";

    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject dragonPrefab;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnKnight()
    {
        PhotonNetwork.Instantiate(knightPrefab.name, new Vector2(-3f, -0.5f), knightPrefab.transform.rotation);

        // TODO: spawn enemies in separate script
        PhotonNetwork.Instantiate(Resources.Load<GameObject>("Knight Enemy").name, new Vector2(6f, 2f), Quaternion.identity);
    }

    private void SpawnDragon()
    {
        PhotonNetwork.Instantiate(dragonPrefab.name, new Vector2(-5f, 3f), dragonPrefab.transform.rotation);

        // TODO: spawn enemies in separate script
        PhotonNetwork.Instantiate(Resources.Load<GameObject>("Dragon Enemy").name, new Vector2(6f, 4f), Quaternion.identity);
    }

    private void SpawnPlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PLAYER_PROPERTIES_TYPE_KEY))
        {
            SpawnPlayer((PlayerType)PhotonNetwork.
                LocalPlayer.CustomProperties[PLAYER_PROPERTIES_TYPE_KEY]);
        }
        else
        {
            PlayerType playerType = (PlayerType)PhotonNetwork.
                CurrentRoom.CustomProperties[RoomManager.ROOM_PROPERTIES_MISSING_TYPE_KEY];

            SpawnPlayer(playerType);
            RoleSelectManager.SelectRole(playerType);
        }
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
