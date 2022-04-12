using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using PlayerType = RoleSelectManager.PlayerType;

public class HintDisplay : MonoBehaviour
{
    [SerializeField] private int secondsToTimeout;
    [SerializeField] private PhotonView photonView;

    [Header("Hints")]

    [SerializeField] private List<GameObject> knightHints;
    [SerializeField] private List<GameObject> dragonHints;

    private Dictionary<PlayerType, List<GameObject>> playerTypeToHintsDictionary;

    private Coroutine timeout;
    private bool isEnabled;

    private void OnValidate()
    {
        if (secondsToTimeout > 0 && photonView == null)
        {
            Debug.LogWarning("Photon View is required to be able to reveal hint after timeout!");
        }
    }

    private void Awake()
    {
        playerTypeToHintsDictionary = new Dictionary<PlayerType, List<GameObject>>()
        {
            { PlayerType.KNIGHT, knightHints },
            { PlayerType.DRAGON, dragonHints }
        };

        isEnabled = (bool)PhotonNetwork.CurrentRoom
            .CustomProperties[LevelSelectManager.ROOM_PROPERTIES_HINTS_ENABLED];

       if (isEnabled && secondsToTimeout == 0)
        {
            RevealLocalHint();
        }
    }

    public void StartTimer()
    {
        if (isEnabled && secondsToTimeout > 0)
        {
            timeout = StartCoroutine(RevealHintAfterTimeout());
        }
    }

    public void StopTimerIfRunning()
    {
        if (timeout != null)
        {
            StopCoroutine(timeout);
            timeout = null;
        }
    }

    private IEnumerator RevealHintAfterTimeout()
    {
        yield return new WaitForSeconds(secondsToTimeout);

        photonView.RPC("RPC_RevealHint", RpcTarget.All);
        timeout = null;

        yield return null;
    }

    private void RevealLocalHint()
    {
        PlayerType localPlayerType = PlayerSpawner.GetLocalPlayerType();
        foreach (GameObject hintObj in playerTypeToHintsDictionary[localPlayerType])
        {
            hintObj.SetActive(true);
        }
    }

    [PunRPC]
    private void RPC_RevealHint()
    {
        RevealLocalHint();
    }
}
