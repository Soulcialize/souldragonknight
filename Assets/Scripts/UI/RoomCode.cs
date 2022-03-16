using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomCode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textObject;

    private void Start()
    {
        textObject.text = PhotonNetwork.CurrentRoom.Name;
    }

}
