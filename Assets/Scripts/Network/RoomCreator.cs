using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RoomCreator : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private string roomSceneName;

    public void CreateRoom()
    {
        Debug.Log($"Creating room ({roomNameInputField})");
        PhotonNetwork.CreateRoom(roomNameInputField.text);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(roomSceneName);
    }
}
