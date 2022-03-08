using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoleSelectManager : MonoBehaviourPunCallbacks
{
    public enum PlayerType { KNIGHT, DRAGON };

    [SerializeField] private Button levelSelectButton;
    [SerializeField] private string levelSelectSceneName;
    [SerializeField] private GameObject[] yourRoleIndicator;
    [SerializeField] private GameObject[] partnerRoleIndicator;

    private void Start()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            object playerTypeObj = player.CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];
            if (playerTypeObj != null)
            {
                IndicatorUpdate(player, (PlayerType)playerTypeObj);
            }
        }
    }
    public static void SelectRole(PlayerType playerType)
    {
        Hashtable playerProperties = new Hashtable();
        playerProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY] = playerType;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        PlayerType playerType = (PlayerType)targetPlayer.CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];
        Debug.Log($"Player {targetPlayer.ActorNumber} chose {System.Enum.GetName(typeof(PlayerType), playerType)}");

        levelSelectButton.interactable = CanPickLevel();
        IndicatorUpdate(targetPlayer, playerType);
    }

    private bool CanPickLevel()
    {
        HashSet<PlayerType> selectedRoles = new HashSet<PlayerType>();

        // check if every player currently in the room has selected a role
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            object playerTypeObj = player.CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];
            if (playerTypeObj == null)
            {
                return false;
            }

            selectedRoles.Add((PlayerType)playerTypeObj);
        }

        // check if every role has been selected
        return selectedRoles.Count == System.Enum.GetValues(typeof(PlayerType)).Length;
    }

    private void IndicatorUpdate(Player player, PlayerType type)
    {
        if (player == PhotonNetwork.LocalPlayer)
        {
            yourRoleIndicator[(int) type].SetActive(true);
            yourRoleIndicator[1-(int) type].SetActive(false);
        }
        else
        {
            partnerRoleIndicator[(int) type].SetActive(true);
            partnerRoleIndicator[1 - (int) type].SetActive(false);
        }
    }

    public void MoveToLevelSelect()
    {
        photonView.RPC("RPC_LoadLevelSelect", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_LoadLevelSelect()
    {
        PhotonNetwork.LoadLevel(levelSelectSceneName);
    }
}
