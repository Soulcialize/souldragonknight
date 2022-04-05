using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject warningIndicator;
    [SerializeField] private GameObject warningText;

    private float maxValue = 1.0f;
    private Coroutine regen;
    private Coroutine warning;

    private void Start()
    {
        slider.value = maxValue;
    }

    public void UpdateValue(float amount)
    {
        if (regen != null)
        {
            StopCoroutine(regen);
        }

        slider.value = amount;
    }

    public void StartRegeneration(float regenSpeed )
    {
        regen = StartCoroutine(Regenerate(regenSpeed));
    }

    public void StopRegeneration()
    {
        if (regen != null)
        {
            StopCoroutine(regen);
            regen = null;
        }
    }

    public void FlashWarningIfNotRunning()
    {
        if (warning == null)
        {
            warning = StartCoroutine(FlashWarning());
        }
    }

    public IEnumerator Regenerate(float regenSpeed)
    {
        while (slider.value < maxValue)
        {
            slider.value = Mathf.Min(maxValue, 
                slider.value + Time.deltaTime * regenSpeed);
            yield return null;
        }
        regen = null;
    }

    private IEnumerator FlashWarning()
    {
        warningText.SetActive(true);

        for (int i = 0; i < 5; i++)
        {
            warningIndicator.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            warningIndicator.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }

        warningText.SetActive(false);
        warning = null;
    }
}
