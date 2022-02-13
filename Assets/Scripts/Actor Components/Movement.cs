using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected PhotonView photonView;
    [SerializeField] protected Rigidbody2D rigidbody2d;
    [SerializeField] protected Animator animator;


    protected virtual void Start()
    {

    }

    protected virtual void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            UpdateMovement();
        }
    }

    protected abstract void UpdateMovement();
}
