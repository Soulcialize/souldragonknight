using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private string lobbySceneName;
    [SerializeField] private GameObject optionsMenu;

    public void LoadLobbyScene()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.MENU_BUTTON);
        SceneManager.LoadScene(lobbySceneName);
    }

    public void OpenOptionsMenu()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.MENU_BUTTON);
        optionsMenu.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.MENU_BUTTON);
        optionsMenu.SetActive(false);
    }

    public void ExitGame()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.MENU_BUTTON);
        Application.Quit();
    }
}
