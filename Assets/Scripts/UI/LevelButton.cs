using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LevelButton : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject clientIndicator;
    [SerializeField] private GameObject partnerIndicator;
    [SerializeField] private int levelNumber;

    public void SetInteractable(bool isInteractable)
    {
        button.interactable = isInteractable;
    }

    public void UpdateClientIndicator(bool isSelected)
    {
        clientIndicator.SetActive(isSelected);
    }

    public void UpdatePartnerIndicator(bool isSelected)
    {
        partnerIndicator.SetActive(isSelected);
    }

    public void DisablePartnerIndicator()
    {
        partnerIndicator.SetActive(false);
    }
}
