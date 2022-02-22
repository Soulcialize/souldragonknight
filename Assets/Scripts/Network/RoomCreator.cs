using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class RoomCreator : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private string roomSceneName;

    public void CreateRoom()
    {
        Debug.Log($"Creating room ({roomNameInputField.text})");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(roomSceneName);
    }
}
