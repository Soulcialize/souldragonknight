using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LevelSelectManager : MonoBehaviourPunCallbacks
{
    public static readonly string ROOM_PROPERTIES_LEVELS_CLEARED = "levelsCleared";
    public static readonly string ROOM_PROPERTIES_HINTS_ENABLED = "hintsEnabled";
    public static readonly string PLAYER_PROPERTIES_LEVEL_SELECTED = "levelSelected";

    [SerializeField] private Button startButton;
    [SerializeField] private Toggle hintsToggle;
    [SerializeField] private LevelButton[] levelSelectButtons;
    [SerializeField] private string roleSelectSceneName;
    [SerializeField] private string[] gameSceneNames;

    private int selectedLevel;
    private int partnerSelectedLevel;

    private void Start()
    {
        int levelsCleared = (int)PhotonNetwork.CurrentRoom
            .CustomProperties[ROOM_PROPERTIES_LEVELS_CLEARED];

        hintsToggle.SetIsOnWithoutNotify((bool)PhotonNetwork.CurrentRoom
            .CustomProperties[ROOM_PROPERTIES_HINTS_ENABLED]);

        for (int i = 0; i <= levelsCleared; i++)
        {
            levelSelectButtons[i].SetInteractable(true);
        }
    }
    public void SelectLevel(int levelNumber)
    {
        AudioManagerSynced.Instance.PlaySoundFx(true, SoundFx.LibraryIndex.ROLE_LEVEL_BUTTON);

        if (selectedLevel != levelNumber)
        {
            Hashtable playerProperties = new Hashtable();
            playerProperties[PLAYER_PROPERTIES_LEVEL_SELECTED] = levelNumber;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

            if (selectedLevel > 0)
            {
                levelSelectButtons[selectedLevel - 1].UpdateClientIndicator(false);
            }

            selectedLevel = levelNumber;
            levelSelectButtons[levelNumber - 1].UpdateClientIndicator(true);
            startButton.interactable = CanStart();

            photonView.RPC("RPC_UpdatePartnerIndicator", RpcTarget.Others, levelNumber);
        }
    }

    public static void SetLevelsCleared(int levelsCleared)
    {
        RoomManager.UpdateRoomProperty(ROOM_PROPERTIES_LEVELS_CLEARED, levelsCleared);
    }

    public void OnHintToggleChange()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.MENU_BUTTON);
        SetHintsEnabled(hintsToggle.isOn);
        photonView.RPC("RPC_FlipHintToggle", RpcTarget.Others);
    }

    public static void SetHintsEnabled(bool isEnabled)
    {
        RoomManager.UpdateRoomProperty(ROOM_PROPERTIES_HINTS_ENABLED, isEnabled);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        PhotonNetwork.LoadLevel(roleSelectSceneName);
        ResetLevelChoice();
    }

    private bool CanStart()
    {
        return selectedLevel > 0 && (selectedLevel == partnerSelectedLevel);
    }

    public static void ResetLevelChoice()
    {
        Hashtable playerProperties = new Hashtable();
        playerProperties[PLAYER_PROPERTIES_LEVEL_SELECTED] = null;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    public void StartGame()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.MENU_BUTTON);
        AudioManagerSynced.Instance.StopMusic(true, Music.LibraryIndex.MENU_BACKGROUND_MUSIC);
        AudioManagerSynced.Instance.PlayMusic(true, Music.LibraryIndex.INGAME_BACKGROUND_MUSIC);

        Debug.Log("Starting game...");
        photonView.RPC("RPC_LoadGameLevel", RpcTarget.All);
    }

    public void ReturnToRoleSelect()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.MENU_BUTTON);
        photonView.RPC("RPC_LoadRoleSelectLevel", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_UpdatePartnerIndicator(int levelNumber)
    {
        if (partnerSelectedLevel > 0)
        {
            levelSelectButtons[partnerSelectedLevel - 1].UpdatePartnerIndicator(false);   
        }

        partnerSelectedLevel = levelNumber;
        levelSelectButtons[levelNumber - 1].UpdatePartnerIndicator(true);
        startButton.interactable = CanStart();
    }

    [PunRPC]
    private void RPC_LoadGameLevel()
    {
        PhotonNetwork.LoadLevel(gameSceneNames[selectedLevel - 1]);
        ResetLevelChoice();
    }

    [PunRPC]
    private void RPC_LoadRoleSelectLevel()
    {
        PhotonNetwork.LoadLevel(roleSelectSceneName);
        ResetLevelChoice();
    }

    [PunRPC]
    private void RPC_FlipHintToggle()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.MENU_BUTTON);
        hintsToggle.SetIsOnWithoutNotify(!hintsToggle.isOn);
    }
}
