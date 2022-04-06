using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private bool isRequestingRestart = false;
    private bool isRequestingExit = false;

    [Header("Scene Names")]

    [SerializeField] private string gameSceneName;
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private string roomSceneName;

    [Header("Events")]

    [SerializeField] private UnityEvent requestRestartEvent;
    [SerializeField] private UnityEvent cancelRestartEvent;
    [SerializeField] private UnityEvent requestExitEvent;
    [SerializeField] private UnityEvent cancelExitEvent;

    public static void UpdateRoomProperty(string key, object value)
    {
        Hashtable roomProperty = new Hashtable();
        roomProperty[key] = value;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperty);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.ActorNumber} has left the game");

        PhotonNetwork.LoadLevel(roomSceneName);
        RoleSelectManager.HadDisconnect = true;
        AudioManager.Instance.StopMusic(Music.LibraryIndex.INGAME_BACKGROUND_MUSIC);
        AudioManager.Instance.PlayMusic(Music.LibraryIndex.MENU_BACKGROUND_MUSIC);

        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void LeaveRoom()
    {
        RoleSelectManager.ResetRole();
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        AudioManager.Instance.StopMusic(Music.LibraryIndex.INGAME_BACKGROUND_MUSIC);
        AudioManager.Instance.PlayMusic(Music.LibraryIndex.MENU_BACKGROUND_MUSIC);
        PhotonNetwork.LoadLevel(mainMenuSceneName);
    }

    public void AttemptRestart()
    {
        if (isRequestingRestart)
        {
            RestartLevel();
        }
        else
        {
            RequestRestart();
            requestRestartEvent.Invoke();
        }
    }

    public void AttemptExit()
    {
        if (isRequestingExit)
        {
            ExitToRoom();
        }
        else
        {
            RequestExit();
            requestExitEvent.Invoke();
        }
    }

    public void CancelRestartRequest()
    {
        photonView.RPC("RPC_CancelRestartRequest", RpcTarget.Others);
        cancelRestartEvent.Invoke();
    }

    public void CancelExitRequest()
    {
        photonView.RPC("RPC_CancelExitRequest", RpcTarget.Others);
        cancelExitEvent.Invoke();
    }

    private void RequestRestart()
    {
        photonView.RPC("RPC_RequestRestart", RpcTarget.Others);
    }

    private void RequestExit()
    {
        photonView.RPC("RPC_RequestExit", RpcTarget.Others);
    }

    private void RestartLevel()
    {
        photonView.RPC("RPC_LoadGameLevel", RpcTarget.All);
    }

    private void ExitToRoom()
    {
        AudioManagerSynced.Instance.StopMusic(Music.LibraryIndex.INGAME_BACKGROUND_MUSIC);
        AudioManagerSynced.Instance.PlayMusic(Music.LibraryIndex.MENU_BACKGROUND_MUSIC);
        photonView.RPC("RPC_LoadRoomLevel", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_RequestRestart()
    {
        Debug.Log("Partner is requesting restart");
        isRequestingRestart = true;
        isRequestingExit = false;
    }

    [PunRPC]
    private void RPC_RequestExit()
    {
        Debug.Log("Partner is requesting to exit");
        isRequestingRestart = false;
        isRequestingExit = true;
    }

    [PunRPC]
    private void RPC_CancelRestartRequest()
    {
        Debug.Log("Partner is no longer requesting restart");
        isRequestingRestart = false;
    }

    [PunRPC]
    private void RPC_CancelExitRequest()
    {
        Debug.Log("Partner is no longer requesting to exit");
        isRequestingExit = false;
    }

    [PunRPC]
    public void RPC_LoadGameLevel()
    {
        PhotonNetwork.LoadLevel(gameSceneName);
    }

    [PunRPC]
    public void RPC_LoadRoomLevel()
    {
        PhotonNetwork.LoadLevel(roomSceneName);
    }
}
