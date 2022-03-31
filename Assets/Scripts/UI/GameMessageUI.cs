using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using PlayerType = RoleSelectManager.PlayerType;

public enum MessageType { RESTART, EXIT }

public class GameMessageUI : MonoBehaviour
{
    public static readonly string MESSAGE_REQUESTING = " is requesting to";
    public static readonly string MESSAGE_CANCELLED= " is no longer requesting to";
    public static readonly string MESSAGE_RESTART = " restart.";
    public static readonly string MESSAGE_EXIT = " exit to role select.";

    private string playerString;

    [SerializeField] private GameMessage restartMessage;
    [SerializeField] private GameMessage exitMessage;

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

    public void UpdateRestartMessage(bool isRequesting)
    {
        string message = playerString;

        if (isRequesting)
        {
            message += MESSAGE_REQUESTING + MESSAGE_RESTART;
            restartMessage.UpdateMessage(message, false);
        } 
        else
        {
            message += MESSAGE_CANCELLED + MESSAGE_RESTART;
            restartMessage.UpdateMessage(message, true);
        }
    }

    public void UpdateExitMessage(bool isRequesting)
    {
        string message = playerString;

        if (isRequesting)
        {
            message += MESSAGE_REQUESTING + MESSAGE_EXIT;
            exitMessage.UpdateMessage(message, false);
        }
        else
        {
            message += MESSAGE_CANCELLED + MESSAGE_EXIT;
            exitMessage.UpdateMessage(message, true);
        }
    }
}
