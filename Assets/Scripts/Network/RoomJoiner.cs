using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RoomJoiner : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private string roomSceneName;

    public void JoinRoom()
    {
        Debug.Log($"Joining room ({roomNameInputField})");
        PhotonNetwork.JoinRoom(roomNameInputField.text);
    }
}
