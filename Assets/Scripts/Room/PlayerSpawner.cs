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
        PhotonNetwork.Instantiate("Knight", new Vector2(-3f, 1.56f), knightPrefab.transform.rotation);

        // TODO: consider doing this stuff in other scripts
        BackgroundManager.Instance.ActivateSoulWorldBackground();
        PhotonNetwork.Instantiate("Knight Enemy", new Vector2(6f, 1.56f), Quaternion.identity);
        PhotonNetwork.Instantiate("Knight Enemy Ranged", new Vector2(6.5f, 5.5f), Quaternion.identity);
    }

    private void SpawnDragon()
    {
        PhotonNetwork.Instantiate("Dragon", new Vector2(-5f, 4f), dragonPrefab.transform.rotation);

        // TODO: consider doing this stuff in other scripts
        BackgroundManager.Instance.ActivateRealWorldBackground();
        PhotonNetwork.Instantiate("Dragon Enemy", new Vector2(6f, 4.5f), Quaternion.identity);
        PhotonNetwork.Instantiate("Dragon Enemy Ranged", new Vector2(6.5f, 2f), Quaternion.identity);
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
