using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class Health : MonoBehaviour
{
    [SerializeField] private int healthPoints;
    [SerializeField] private UnityEvent decrementHealthEvent;
    [SerializeField] private PhotonView photonView;

    public int HealthPoints { get => healthPoints; }

    public UnityEvent DecrementHealthEvent { get => decrementHealthEvent; }

    public void Decrement()
    {
        photonView.RPC("RPC_Decrement", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Decrement()
    {
        healthPoints = Mathf.Max(0, healthPoints - 1);
        decrementHealthEvent.Invoke();
    }

    public bool IsZero()
    {
        return healthPoints == 0;
    }
}
