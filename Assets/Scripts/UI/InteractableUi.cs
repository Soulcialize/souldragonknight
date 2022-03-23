using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textObj;

    public void DisplayPrompt(string prompt, Vector2 localPosition)
    {
        textObj.text = prompt;
        transform.localPosition = localPosition;
        gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        gameObject.SetActive(false);
    }
}
