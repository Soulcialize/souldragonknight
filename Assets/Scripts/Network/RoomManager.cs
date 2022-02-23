using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public enum PlayerType { DRAGON, KNIGHT };

    [SerializeField] private string gameSceneName;
    [SerializeField] private string mainMenuSceneName;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public static void UpdateRoomPropsMissingPlayer(PlayerType playerType)
    {
        Hashtable hash = new Hashtable();
        hash.Add("MissingPlayer", playerType);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    public void RestartLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            PhotonNetwork.LoadLevel(gameSceneName);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerType playerType = (PlayerType) otherPlayer.CustomProperties["PlayerType"];
        UpdateRoomPropsMissingPlayer(playerType);

        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(mainMenuSceneName);
        base.OnLeftRoom();
    }
}
