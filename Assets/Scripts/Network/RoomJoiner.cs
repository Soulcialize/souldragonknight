using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomJoiner : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private string roomSceneName;
    [SerializeField] private string gameSceneName;

    public void JoinRoom()
    {
        Debug.Log($"Joining room ({roomNameInputField.text})");
        PhotonNetwork.JoinRoom(roomNameInputField.text);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(RoomManager.ROOM_PROPERTIES_STATUS_KEY))
        {
            bool hasGameStarted = (bool)PhotonNetwork.
                CurrentRoom.CustomProperties[RoomManager.ROOM_PROPERTIES_STATUS_KEY];

            if (hasGameStarted)
            {
                PhotonNetwork.LoadLevel(gameSceneName);
            } else
            {
                PhotonNetwork.LoadLevel(roomSceneName);
            }
        }
        else
        {
            PhotonNetwork.LoadLevel(roomSceneName);
        }
    }
}
