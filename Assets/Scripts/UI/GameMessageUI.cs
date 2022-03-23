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
    private Coroutine timeoutMessage;

    [SerializeField] PhotonView photonView;
    [SerializeField] private TextMeshProUGUI textObject;

    private void Start()
    {
        PlayerType playerType = PlayerSpawner.GetLocalPlayerType();

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
            timeoutMessage = StartCoroutine(ExpireMessage());
        }

        photonView.RPC("RPC_UpdateMessage", RpcTarget.All, playerString + suffix);
    }

    private void ClearRequestMessageIfExist()
    {
        if (timeoutMessage != null)
        {
            StopCoroutine(timeoutMessage);
            timeoutMessage = null;
        }

        if (textObject.text.Equals(playerString + MESSAGE_CANCELLED_RESTART))
        {
            photonView.RPC("RPC_ResetMessage", RpcTarget.All);
        }
    }

    private IEnumerator ExpireMessage()
    {
        yield return new WaitForSeconds(timerDuration);

        ClearRequestMessageIfExist();
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
