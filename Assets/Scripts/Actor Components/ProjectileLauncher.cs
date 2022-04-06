using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Transform transformToRotate;

    private bool isAimingAtTarget;
    private Transform target;

    private void Update()
    {
        if (isAimingAtTarget)
        {
            UpdateAimDirection();
        }
    }

    public void StartAimingProjectileLauncher(Transform target)
    {
        photonView.RPC("RPC_StartAimingProjectileLauncher", RpcTarget.All,
            target.GetComponent<PhotonView>().ViewID);
    }

    public void StopAimingProjectileLauncher()
    {
        photonView.RPC("RPC_StopAimingProjectileLauncher", RpcTarget.All);
    }

    public void HideProjectileLauncher()
    {
        photonView.RPC("RPC_HideProjectileLauncher", RpcTarget.All);
    }

    private void UpdateAimDirection()
    {
        Vector2 direction = target.position - transformToRotate.position;
        transformToRotate.rotation = Quaternion.AngleAxis(
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg,
            Vector3.forward);
    }

    public void FlipDirection()
    {
        Vector3 localScale = transformToRotate.localScale;
        localScale.x = -localScale.x;
        transformToRotate.localScale = localScale;
    }

    [PunRPC]
    private void RPC_StartAimingProjectileLauncher(int targetPhotonViewId)
    {
        gameObject.SetActive(true);
        target = PhotonView.Find(targetPhotonViewId).transform;
        isAimingAtTarget = true;
    }

    [PunRPC]
    private void RPC_StopAimingProjectileLauncher()
    {
        isAimingAtTarget = false;
        UpdateAimDirection();
    }

    [PunRPC]
    private void RPC_HideProjectileLauncher()
    {
        gameObject.SetActive(false);
        isAimingAtTarget = false;
    }
}
