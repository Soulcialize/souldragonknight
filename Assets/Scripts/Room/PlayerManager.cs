using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    private readonly int deadPlayerLimit = 2;

    private static PlayerManager _instance;
    public static PlayerManager Instance { get => _instance; }

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private PhotonView photonView;

    private int deadPlayerCount = 0;

    void Awake()
    {
        // singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void IncrementDeathCount()
    {
        photonView.RPC("RPC_IncrementDeath", RpcTarget.All);
    }

    public void DecrementDeathCount()
    {
        photonView.RPC("RPC_DecrementDeath", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_IncrementDeath()
    {
        deadPlayerCount++;

        if (deadPlayerCount >= deadPlayerLimit)
        {
            StartCoroutine(InitateGameOver());
        }
    }

    [PunRPC]
    private void RPC_DecrementDeath()
    {
        deadPlayerCount--;
    }

    private IEnumerator InitateGameOver()
    {
        AudioManager.Instance.StopMusic(Music.LibraryIndex.INGAME_BACKGROUND_MUSIC, true);
        deathScreen.SetActive(true);

        yield return new WaitForSeconds(2f);
        
        AudioManager.Instance.PlayMusic(Music.LibraryIndex.DEFEAT_MUSIC, true, 1f);
    }
}
