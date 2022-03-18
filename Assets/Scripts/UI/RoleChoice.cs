using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

using PlayerType = RoleSelectManager.PlayerType;

public class RoleChoice : MonoBehaviour
{
    public static readonly string PLAYER_CHOICE_KNIGHT = "KNIGHT";
    public static readonly string PLAYER_CHOICE_DRAGON = "DRAGON";
    public static readonly string PLAYER_CHOICE_ERROR = "Something went wrong. Please reselect your role.";

    [SerializeField] private TextMeshProUGUI textObject;

    private void Start()
    {
        PlayerType playerType = (PlayerType)PhotonNetwork.LocalPlayer.
            CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];

        switch (playerType)
        {
            case PlayerType.KNIGHT:
                textObject.text += PLAYER_CHOICE_KNIGHT;
                break;
            case PlayerType.DRAGON:
                textObject.text += PLAYER_CHOICE_DRAGON;
                break;
            default:
                textObject.text = PLAYER_CHOICE_ERROR;
                return;
        }
    }
}
