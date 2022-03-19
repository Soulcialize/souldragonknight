using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

using PlayerType = RoleSelectManager.PlayerType;

public class GameMessageUI : MonoBehaviour
{
    public static readonly string MESSAGE_REQUESTING_RESTART = " is requesting to restart.";
    public static readonly string MESSAGE_CANCELLED_RESTART = " is no longer requesting to restart.";

    private readonly float timerDuration = 3.0f;

    private string playerString;
    private float timer = 0f;
    private bool isTimerRunning = false;

    [SerializeField] PhotonView photonView;
    [SerializeField] private TextMeshProUGUI textObject;

    private void Start()
    {
        PlayerType playerType = (PlayerType)PhotonNetwork.LocalPlayer.
            CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];

        switch (playerType)
        {
            case PlayerType.KNIGHT:
                playerString = RoleChoice.PLAYER_CHOICE_KNIGHT;
                break;
            case PlayerType.DRAGON:
                playerString = RoleChoice.PLAYER_CHOICE_DRAGON;
                break;
            default:
                throw new System.ArgumentException("Unknown player type");
        }
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            } 
            else
            {
                isTimerRunning = false;
                ClearRequestMessageIfExist();
            }
        }
    }

    public void UpdateRestartMessage(bool isRestarting)
    {
        string suffix;

        if (isRestarting)
        {
            suffix = MESSAGE_REQUESTING_RESTART;
        } 
        else
        {
            suffix = MESSAGE_CANCELLED_RESTART;
            StartTimer();
        }

        photonView.RPC("RPC_UpdateMessage", RpcTarget.All, playerString + suffix);
    }

    private void ClearRequestMessageIfExist()
    {
        if (textObject.text.Equals(playerString + MESSAGE_CANCELLED_RESTART))
        {
            photonView.RPC("RPC_ResetMessage", RpcTarget.All);
        }
    }

    private void StartTimer()
    {
        isTimerRunning = true;
        timer = timerDuration;
    }

    [PunRPC]
    private void RPC_UpdateMessage(string message)
    {
        textObject.text = message;
    }

    [PunRPC]
    private void RPC_ResetMessage()
    {
        textObject.text = "";
    }
}
