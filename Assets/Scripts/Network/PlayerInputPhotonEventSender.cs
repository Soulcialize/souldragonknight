using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerInputPhotonEventSender : MonoBehaviourPunCallbacks
{
    private static PlayerInputPhotonEventSender instance;
    public static PlayerInputPhotonEventSender Instance { get => instance; }

    private void Awake()
    {
        // singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void SendInputEvent(byte eventCode, int playerViewId, params object[] data)
    {
        object[] eventData = new object[data.Length + 1];
        eventData[0] = playerViewId;
        data.CopyTo(eventData, 1);
        PhotonNetwork.RaiseEvent(eventCode, eventData, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void SendMoveInputEvent(int playerViewId, float moveInput)
    {
        SendInputEvent(NetworkEvents.PLAYER_MOVE_EVENT, playerViewId, moveInput);
    }

    public void SendJumpInputEvent(int playerViewId)
    {
        SendInputEvent(NetworkEvents.PLAYER_JUMP_EVENT, playerViewId);
    }

    public void SendAttackInputEvent(int playerViewId)
    {
        SendInputEvent(NetworkEvents.PLAYER_ATTACK_EVENT, playerViewId);
    }
}
