using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;

    [Space(10)]

    [SerializeField] private UnityEvent playerEnteredEvent;

    private int numEntries = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // should only ever be player since this trigger only collides with knight and dragon layers
        photonView.RPC("RPC_PlayerEnteredTriggerHandler", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_PlayerEnteredTriggerHandler()
    {
        // only run on the master client
        if (numEntries == 0)
        {
            numEntries++;
            playerEnteredEvent.Invoke();
        }
    }
}
