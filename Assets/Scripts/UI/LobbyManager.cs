using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private string startSceneName;

    public void LoadStartScene()
    {
        SceneManager.LoadScene(startSceneName);
    }
}
