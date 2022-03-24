using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomCreator : MonoBehaviourPunCallbacks
{
    public static readonly int LOBBY_ALREADY_EXISTS_ERROR_NAME = 32766;

    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private string roomSceneName;
    [SerializeField] private TextMeshProUGUI errorMessage;

    public void CreateRoom()
    {
        if (roomNameInputField.text == "")
        {
            errorMessage.text = "Please enter a room name.";
        }
        else
        {
            Debug.Log($"Creating room ({roomNameInputField.text})");

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions, null);
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        LevelSelectManager.SetLevelsCleared(0);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        base.OnCreateRoomFailed(returnCode, message);
        if (returnCode == LOBBY_ALREADY_EXISTS_ERROR_NAME) {
            errorMessage.text = "A lobby with that name already exists!";
        }

    }
    
}
