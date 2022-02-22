using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ServerReconnector : MonoBehaviourPunCallbacks
{
    [SerializeField] private string loadingSceneName;

    public void Reconnect() {
        SceneManager.LoadScene(loadingSceneName);
    }   
}
