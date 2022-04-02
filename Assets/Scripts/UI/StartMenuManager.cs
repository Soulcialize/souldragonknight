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
        SceneManager.LoadScene(lobbySceneName);
    }

    public void OpenOptionsMenu()
    {
        
    }

    public void CloseOptionsMenu()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
