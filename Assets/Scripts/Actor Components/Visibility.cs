using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Coroutine fadeCoroutine;

    public bool IsVisible { get; private set; }

    public void Hide()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        IsVisible = false;
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
    }

    public void Reveal()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        IsVisible = true;
        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
    }

    public void RevealBriefly(float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        Reveal();
        fadeCoroutine = StartCoroutine(FadeVisibility(false, duration));
    }

    private IEnumerator FadeVisibility(bool toReveal, float duration)
    {
        Color initialColor = spriteRenderer.color;
        Color targetColor = new Color(
            initialColor.r, initialColor.g, initialColor.b, toReveal ? 255f : 0f);

        float timePassed = 0f;
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(initialColor, targetColor, timePassed / duration);
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }
}
