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

    private PlayerType playerType;

    [SerializeField] PhotonView photonView;
    [SerializeField] private TextMeshProUGUI textObject;

    private void Start()
    {
        playerType = (PlayerType)PhotonNetwork.LocalPlayer.
            CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];
    }

    public void UpdateRestartMessage(bool isRestarting)
    {
        string message = "";
        string suffix = isRestarting ? MESSAGE_REQUESTING_RESTART : MESSAGE_CANCELLED_RESTART;

        switch (playerType)
        {
            case PlayerType.KNIGHT:
                message = RoleChoice.PLAYER_CHOICE_KNIGHT + suffix;
                break;
            case PlayerType.DRAGON:
                message = RoleChoice.PLAYER_CHOICE_DRAGON + suffix;
                break;
            default:
                throw new System.ArgumentException("Unknown player type");
        }

        photonView.RPC("RPC_UpdateMessage", RpcTarget.All, message);
    }

    [PunRPC]
    private void RPC_UpdateMessage(string message)
    {
        textObject.text = message;
    }
}
