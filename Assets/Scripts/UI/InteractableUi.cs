using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractableUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private GameObject interactionProgress;
    [SerializeField] private Image interactionProgressFill;

    private bool isFlipped = false;

    public void DisplayPrompt(string prompt, float interactionDuration, Vector2 localPosition)
    {
        interactionText.text = prompt;
        transform.localPosition = localPosition;
        if (isFlipped)
        {
            FlipPrompt(false);
        }

        gameObject.SetActive(true);
        SetInteractionProgressFill(0f);
        interactionProgress.SetActive(interactionDuration > 0f);
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

    public void SetInteractionProgressFill(float percentageFilled)
    {
        interactionProgressFill.fillAmount = percentageFilled;
    }
}
