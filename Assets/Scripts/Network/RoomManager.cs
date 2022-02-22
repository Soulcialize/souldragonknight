using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string mainMenuSceneName;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

    }

    public void RestartLevel()
    {
        /*if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            PhotonNetwork.LoadLevel(gameSceneName);
        }*/
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
