using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textObject;
    [SerializeField] private float timerDuration = 3.0f;
    [SerializeField] private PhotonView photonView;

    private bool hasPartnerMessage = false;
    private Coroutine timeoutMessage;

    public void UpdateMessage(string message, bool hasTimeout)
    {
        textObject.text = message;
        StopTimeoutIfRunning();

        if (hasTimeout)
        {
            timeoutMessage = StartCoroutine(ClearMessageAfterTimeout());
        }

        photonView.RPC("RPC_SyncMessage", RpcTarget.Others, message);
    }

    public void ClearOwnMessageIfExist()
    {
        if (!hasPartnerMessage && textObject.text != "")
        {
            ClearMessage();
        }
    }

    private void ClearMessage()
    {
        textObject.text = "";

        photonView.RPC("RPC_SyncMessage", RpcTarget.Others, "");
    }

    private void StopTimeoutIfRunning()
    {
        if (timeoutMessage != null)
        {
            StopCoroutine(timeoutMessage);
            timeoutMessage = null; 
        }
    }

    private IEnumerator ClearMessageAfterTimeout()
    {
        yield return new WaitForSeconds(timerDuration);
        
        StopTimeoutIfRunning();
        ClearMessage();
    }

    [PunRPC]
    public void RPC_SyncMessage(string message)
    {
        textObject.text = message;
        StopTimeoutIfRunning();

        hasPartnerMessage = (message != "");
    }
}
