using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerInputPhotonEventReceiver : MonoBehaviourPunCallbacks
{
    private static PlayerInputPhotonEventReceiver instance;
    public static PlayerInputPhotonEventReceiver Instance { get => instance; }

    private Dictionary<int, PlayerController> viewIdToPlayerDictionary = new Dictionary<int, PlayerController>();

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

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += ReceiveMoveInputEvent;
        PhotonNetwork.NetworkingClient.EventReceived += ReceiveJumpInputEvent;
        PhotonNetwork.NetworkingClient.EventReceived += ReceiveAttackInputEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= ReceiveMoveInputEvent;
        PhotonNetwork.NetworkingClient.EventReceived -= ReceiveJumpInputEvent;
        PhotonNetwork.NetworkingClient.EventReceived -= ReceiveAttackInputEvent;
    }

    public void AddPlayerViewId(int viewId, PlayerController player)
    {
        viewIdToPlayerDictionary.Add(viewId, player);
    }

    public void RemovePlayerViewId(int viewId)
    {
        viewIdToPlayerDictionary.Remove(viewId);
    }

    private void ReceiveMoveInputEvent(EventData obj)
    {
        if (obj.Code != NetworkEvents.PLAYER_MOVE_EVENT)
        {
            return;
        }

        object[] data = (object[])obj.CustomData;
        int viewId = (int)data[0];
        float moveInput = (float)data[1];
        viewIdToPlayerDictionary[viewId].Move(moveInput);
    }

    private void ReceiveJumpInputEvent(EventData obj)
    {
        if (obj.Code != NetworkEvents.PLAYER_JUMP_EVENT)
        {
            return;
        }

        object[] data = (object[])obj.CustomData;
        int viewId = (int)data[0];
        viewIdToPlayerDictionary[viewId].JumpGrounded();
    }

    private void ReceiveAttackInputEvent(EventData obj)
    {
        if (obj.Code != NetworkEvents.PLAYER_ATTACK_EVENT)
        {
            return;
        }

        object[] data = (object[])obj.CustomData;
        int viewId = (int)data[0];
        viewIdToPlayerDictionary[viewId].Attack();
    }
}
