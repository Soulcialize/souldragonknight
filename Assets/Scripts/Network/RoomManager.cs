using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static readonly string ROOM_PROPERTIES_MISSING_TYPE_KEY = "MissingType";
    public static readonly string ROOM_PROPERTIES_STATUS_KEY = "HasGameStarted";

    [SerializeField] private string gameSceneName;
    [SerializeField] private string mainMenuSceneName;

    public static void UpdateRoomProperty(string key, object value)
    {
        Hashtable roomProperty = new Hashtable();
        roomProperty[key] = value;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperty);
    }

    public void RestartLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            base.photonView.RPC("RPC_LoadLevel", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RPC_LoadLevel()
    {
        PhotonNetwork.LoadLevel(gameSceneName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.ActorNumber} has left the game");

        UpdateRoomProperty(ROOM_PROPERTIES_MISSING_TYPE_KEY, 
            otherPlayer.CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY]);

        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel(mainMenuSceneName);

        Hashtable playerProperties = new Hashtable();
        playerProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY] = null;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }
}
