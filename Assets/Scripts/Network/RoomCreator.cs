using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        RoomManager.UpdateRoomProperty(RoomManager.ROOM_PROPERTIES_STATUS_KEY, false);
    }
}
