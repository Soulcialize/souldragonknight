using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoleSelectManager : MonoBehaviourPunCallbacks
{
    public enum PlayerType { KNIGHT, DRAGON };

    public readonly string ROOM_MESSAGE_PLAYER_JOINED = "A player has joined the room";
    public readonly string ROOM_MESSAGE_PLAYER_LEFT = "Your partner has left the room";

    public static bool HadDisconnect { get; set; }

    [SerializeField] private Button levelSelectButton;
    [SerializeField] private string levelSelectSceneName;
    [SerializeField] private string menuSceneName;
    [SerializeField] private GameObject[] yourRoleIndicator;
    [SerializeField] private GameObject[] partnerRoleIndicator;
    [SerializeField] private TextMeshProUGUI roomMessage;

    private Coroutine messageTimeout;

    private void Start()
    {
        if (HadDisconnect) {
            UpdateRoomMessage(ROOM_MESSAGE_PLAYER_LEFT);
        }

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player.CustomProperties.ContainsKey(PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY))
            {
                PlayerType playerType = (PlayerType)player.CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];
                UpdateIndicator(player, playerType);
            }
        }

        levelSelectButton.interactable = CanPickLevel();
    }

    public static void SelectRole(PlayerType playerType)
    {
        Hashtable playerProperties = new Hashtable();
        playerProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY] = playerType;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    public static void ResetRole()
    {
        Hashtable playerProperties = new Hashtable();
        playerProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY] = null;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (targetPlayer.CustomProperties.ContainsKey(PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY))
        {
            PlayerType playerType = (PlayerType)targetPlayer.CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY];
            Debug.Log($"Player {targetPlayer.ActorNumber} chose {System.Enum.GetName(typeof(PlayerType), playerType)}");

            levelSelectButton.interactable = CanPickLevel();
            UpdateIndicator(targetPlayer, playerType);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        HadDisconnect = false;
        UpdateRoomMessage(ROOM_MESSAGE_PLAYER_JOINED);
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        levelSelectButton.interactable = false;
        partnerRoleIndicator[0].SetActive(false);
        partnerRoleIndicator[1].SetActive(false);

        UpdateRoomMessage(ROOM_MESSAGE_PLAYER_LEFT);
    }

    public void UpdateRoomMessage(string message)
    {
        if (messageTimeout != null)
        {
            StopCoroutine(messageTimeout); 
        }

        messageTimeout = StartCoroutine(DisplayMessageWithTimeout(message));
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

    private void UpdateSelfIndicator(PlayerType type)
    {
        yourRoleIndicator[(int)type].SetActive(true);
        yourRoleIndicator[1 - (int)type].SetActive(false);
    }

    private void UpdatePartnerIndicator(PlayerType type)
    {
        partnerRoleIndicator[(int)type].SetActive(true);
        partnerRoleIndicator[1 - (int)type].SetActive(false);
    }

    private void UpdateIndicator(Player player, PlayerType type)
    {
        if (player == PhotonNetwork.LocalPlayer)
        {
            UpdateSelfIndicator(type);
        } 
        else
        {
            UpdatePartnerIndicator(type);
        }
    }

    public void MoveToLevelSelect()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.MENU_BUTTON);
        photonView.RPC("RPC_LoadLevelSelect", RpcTarget.All);
    }

    public void MoveToMenu()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.MENU_BUTTON);
        ResetRole();
        HadDisconnect = false;

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(menuSceneName);
    }

    private IEnumerator DisplayMessageWithTimeout(string message)
    {
        roomMessage.text = message;
        yield return new WaitForSeconds(3);
        roomMessage.text = "";
        messageTimeout = null;
    }

    [PunRPC]
    private void RPC_LoadLevelSelect()
    {
        PhotonNetwork.LoadLevel(levelSelectSceneName);
    }
}
