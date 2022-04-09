using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameHint : MonoBehaviour
{
    [SerializeField] private int secondsToTimeout;
    [SerializeField] private GameObject gameHint;
    [SerializeField] private PhotonView photonView;

    private Coroutine timeout;
    private bool isEnabled;

    private void Start()
    {
        isEnabled = (bool)PhotonNetwork.CurrentRoom
            .CustomProperties[LevelSelectManager.ROOM_PROPERTIES_HINTS_ENABLED];
        Debug.Log(isEnabled);
    }

    public void StartTimer()
    {
        if (isEnabled)
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

    [PunRPC]
    private void RPC_RevealHint()
    {
        gameHint.SetActive(true);
    }
}
