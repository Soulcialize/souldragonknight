using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class RoleSelectManager : MonoBehaviourPunCallbacks
{
    public enum PlayerType { KNIGHT, DRAGON };

    [SerializeField] private Button startGameButton;
    [SerializeField] private string gameSceneName;
    [SerializeField] private GameObject yourRoleIndicatorKnight;
    [SerializeField] private GameObject yourRoleIndicatorDragon;
    [SerializeField] private GameObject partnerRoleIndicatorKnight;
    [SerializeField] private GameObject partnerRoleIndicatorDragon;

    public void SelectRole(PlayerType playerType)
    {
        Hashtable playerProperties = new Hashtable();
        playerProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY] = playerType;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        SetIndicator();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        PlayerType playerType = (PlayerType)targetPlayer.CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];
        print($"Player {targetPlayer.ActorNumber} chose {System.Enum.GetName(typeof(PlayerType), playerType)}");

        startGameButton.interactable = CanStartGame();
        SetIndicator();
    }

    private bool CanStartGame()
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

    private void SetIndicator()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player == PhotonNetwork.LocalPlayer)
            {
                object playerTypeObj = player.CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];
                if (playerTypeObj == null)
                {
                    break;
                }
                if ((PlayerType) playerTypeObj == PlayerType.KNIGHT)
                {
                    yourRoleIndicatorKnight.SetActive(true);
                }
                else
                {
                    yourRoleIndicatorKnight.SetActive(false);
                }
                if ((PlayerType)playerTypeObj == PlayerType.DRAGON)
                {
                    yourRoleIndicatorDragon.SetActive(true);
                }
                else
                {
                    yourRoleIndicatorDragon.SetActive(false);
                }
            }
            else
            {
                object playerTypeObj = player.CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];
                if (playerTypeObj == null)
                {
                    break;
                }
                if ((PlayerType)playerTypeObj == PlayerType.KNIGHT)
                {
                    partnerRoleIndicatorKnight.SetActive(true);
                }
                else
                {
                    partnerRoleIndicatorKnight.SetActive(false);
                }
                if ((PlayerType)playerTypeObj == PlayerType.DRAGON)
                {
                    partnerRoleIndicatorDragon.SetActive(true);
                }
                else
                {
                    partnerRoleIndicatorDragon.SetActive(false);
                }
            }
        }
    }

    public void StartGame()
    {
        photonView.RPC("RPC_LoadLevel", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_LoadLevel()
    {
        PhotonNetwork.LoadLevel(gameSceneName);
    }
}