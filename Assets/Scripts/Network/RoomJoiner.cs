using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomJoiner : MonoBehaviourPunCallbacks
{
    public static readonly int LOBBY_DOES_NOT_EXIST_ERROR_NAME = 32758;
    public static readonly int LOBBY_FULL_ERROR_NAME = 32765;

    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private string roomSceneName;
    [SerializeField] private TextMeshProUGUI errorMessage;

    public void JoinRoom()
    {
        if (roomNameInputField.text=="")
        {
            errorMessage.text = "Please enter a room name.";
        } else
        {
            Debug.Log($"Joining room ({roomNameInputField.text})");
            PhotonNetwork.JoinRoom(roomNameInputField.text);
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(roomSceneName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        base.OnJoinRoomFailed(returnCode, message);
        if (returnCode == LOBBY_DOES_NOT_EXIST_ERROR_NAME) {
            errorMessage.text = "A lobby with that name does not exist!";
        } else if (returnCode == LOBBY_FULL_ERROR_NAME) {
            errorMessage.text = "The lobby you wish to join is full!";
        }
    }
}
