using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textObj;

    private bool isFlipped = false;

    public void DisplayPrompt(string prompt, Vector2 localPosition)
    {
        textObj.text = prompt;
        transform.localPosition = localPosition;
        if (isFlipped)
        {
            FlipPrompt(false);
        }

        gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        gameObject.SetActive(false);
    }

    public void FlipPrompt(bool setIsFlipped)
    {
        if (setIsFlipped)
        {
            isFlipped = !isFlipped;
        }

        Vector3 localPosition = transform.localPosition;
        localPosition.x = -localPosition.x;
        transform.localPosition = localPosition;
    }
}
