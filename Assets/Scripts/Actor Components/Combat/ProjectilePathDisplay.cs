using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectilePathDisplay : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform projectileOrigin;
    [SerializeField] private LayerMask projectileObstaclesLayer;

    private bool isDrawingProjectilePath = false;
    private Transform target;
    private Vector3[] projectilePathPositions = new Vector3[2];

    private void Awake()
    {
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if (isDrawingProjectilePath)
        {
            UpdateProjectilePath();
        }
    }

    private void UpdateProjectilePath()
    {
        projectilePathPositions[0] = projectileOrigin.position;

        Vector2 direction = target.position - projectileOrigin.position;
        RaycastHit2D projectileHit = Physics2D.Raycast(projectileOrigin.position, direction, Mathf.Infinity, projectileObstaclesLayer);
        projectilePathPositions[1] = projectileHit.collider == null
            ? (Vector2)projectileOrigin.position + direction.normalized * 1000f
            : projectileHit.point;

        lineRenderer.SetPositions(projectilePathPositions);
    }

    public void StartDrawingProjectilePath(Transform target)
    {
        gameObject.SetActive(true);
        photonView.RPC("RPC_StartDrawingProjectilePath", RpcTarget.Others, target.GetComponent<PhotonView>().ViewID);
    }

    public void StopUpdatingProjectilePath()
    {
        photonView.RPC("RPC_StopUpdatingProjectilePath", RpcTarget.Others);
    }

    public void StopDrawingProjectilePath()
    {
        gameObject.SetActive(false);
        photonView.RPC("RPC_StopDrawingProjectilePath", RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_StartDrawingProjectilePath(int targetPhotonViewId)
    {
        target = PhotonView.Find(targetPhotonViewId).transform;

        isDrawingProjectilePath = true;
        gameObject.SetActive(true);
    }

    [PunRPC]
    private void RPC_StopUpdatingProjectilePath()
    {
        isDrawingProjectilePath = false;
        UpdateProjectilePath();
    }

    [PunRPC]
    private void RPC_StopDrawingProjectilePath()
    {
        gameObject.SetActive(false);
    }
}
