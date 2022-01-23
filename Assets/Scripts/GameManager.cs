using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    public enum PlayerType { KNIGHT, DRAGON }

    [SerializeField] private PlayerType playerType;

    public PlayerType CurrPlayerType { get => playerType; }

    private void Awake()
    {
        // singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // first player to join: knight; second player to join: dragon
        playerType = PhotonNetwork.CurrentRoom.PlayerCount % 2 == 0 ? PlayerType.DRAGON : PlayerType.KNIGHT;
    }
}
