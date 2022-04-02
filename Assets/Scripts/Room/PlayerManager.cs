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
        DontDestroyOnLoad(gameObject);
    }


}
