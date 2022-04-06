using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;

    private bool isAimingAtTarget;
    private Transform target;

    private void Update()
    {
        if (isAimingAtTarget)
        {
            UpdateAimDirection();
        }
    }

    public void ShowProjectileLauncher()
    {
        photonView.RPC("RPC_ShowProjectileLauncher", RpcTarget.All);
    }

    public void StartAimingProjectileLauncher(Transform target)
    {
        isAimingAtTarget = true;
        this.target = target;
    }

    public void StopAimingProjectileLauncher()
    {
        isAimingAtTarget = false;
        UpdateAimDirection();
    }

    public void HideProjectileLauncher()
    {
        photonView.RPC("RPC_HideProjectileLauncher", RpcTarget.All);
    }

    private void UpdateAimDirection()
    {
        Vector2 direction = target.position - transform.position;

        // clamp direction to right-facing direction, since left-facing is handled by actor being flipped
        direction = new Vector2(Mathf.Clamp(direction.x, 0f, direction.x), direction.y);

        transform.rotation = Quaternion.AngleAxis(
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg,
            Vector3.forward);
    }

    [PunRPC]
    private void RPC_ShowProjectileLauncher()
    {
        gameObject.SetActive(true);
    }

    [PunRPC]
    private void RPC_HideProjectileLauncher()
    {
        gameObject.SetActive(false);
    }
}
