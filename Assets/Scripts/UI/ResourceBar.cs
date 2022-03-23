using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private float maxValue = 1.0f;
    private Coroutine regen;

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
}
