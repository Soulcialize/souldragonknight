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

    public void SelectRole(PlayerType playerType)
    {
        Hashtable playerProperties = new Hashtable();
        playerProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY] = playerType;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        PlayerType playerType = (PlayerType)targetPlayer.CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];
        print($"Player {targetPlayer.ActorNumber} chose {System.Enum.GetName(typeof(PlayerType), playerType)}");

        startGameButton.interactable = CanStartGame();
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
