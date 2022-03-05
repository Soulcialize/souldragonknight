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

    public void UpdateIndicators(int number, bool isLocalPlayer)
    {
        bool isSameNumber = (number == levelNumber);

        if (isLocalPlayer)
        {
            clientIndicator.SetActive(isSameNumber);
        }
        else
        {
            partnerIndicator.SetActive(isSameNumber);
        }
    }
}
