using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string mainMenuSceneName;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
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
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void LeaveRoom()
    {
        Hashtable playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;
        playerProperties.Remove(PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
