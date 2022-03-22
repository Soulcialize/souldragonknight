using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private float maxValue = 1.0f;
    private Coroutine regen;
    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);

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

    public void StartRegeneration()
    {
        regen = StartCoroutine(Regenerate());
    }

    public IEnumerator Regenerate()
    {
        while (slider.value < maxValue)
        {
            slider.value = Mathf.Min(maxValue, slider.value + maxValue / 50);
            yield return regenTick;
        }
        regen = null;
    }
}
