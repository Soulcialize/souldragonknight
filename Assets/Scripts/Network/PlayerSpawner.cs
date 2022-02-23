using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using PlayerType = RoomManager.PlayerType;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject dragonPrefab;

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            SpawnKnight();
            RoomManager.UpdateRoomPropsMissingPlayer(PlayerType.DRAGON);
            SetPlayerProps(PlayerType.KNIGHT);
        } 
        else
        {
            SpawnPlayer();
        }
 
    }

    private void SpawnKnight()
    {
        PhotonNetwork.Instantiate(knightPrefab.name, new Vector2(-7f, -0.5f), knightPrefab.transform.rotation);
    }

    private void SpawnDragon()
    {
        PhotonNetwork.Instantiate(dragonPrefab.name, new Vector2(-6f, 2f), dragonPrefab.transform.rotation);
    }

    private void SpawnPlayerType(PlayerType playerType)
    {
        if (playerType.Equals(PlayerType.KNIGHT))
        {
            SpawnKnight();
            RoomManager.UpdateRoomPropsMissingPlayer(PlayerType.DRAGON);
        } 
        else
        {
            SpawnDragon();
            RoomManager.UpdateRoomPropsMissingPlayer(PlayerType.KNIGHT);
        }
    }

    private void SpawnPlayer()
    {
        try
        {
            PlayerType playerType = (PlayerType)PhotonNetwork.LocalPlayer.CustomProperties["PlayerType"];

            SpawnPlayerType(playerType);
            SetPlayerProps(playerType);
        }
        catch
        {
            PlayerType missingType = (PlayerType)PhotonNetwork.CurrentRoom.CustomProperties["MissingPlayer"];

            SpawnPlayerType(missingType);
            SetPlayerProps(missingType);
        }
    }

    private void SetPlayerProps(PlayerType playerType)
    {
        Hashtable hash = new Hashtable();
        hash.Add("PlayerType", playerType);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
}
