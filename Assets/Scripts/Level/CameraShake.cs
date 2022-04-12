using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake _instance;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel;

    [SerializeField] private CinemachineVirtualCamera CVCamera;
    [SerializeField] private float defaultCameraSize = 7f;

    public static CameraShake Instance { get => _instance; }

    private void Awake()
    {
        _instance = this;
        cinemachineBasicMultiChannel = CVCamera
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        Debug.Log(cinemachineBasicMultiChannel.m_AmplitudeGain);
    }

    public void Shake(float intensity, float time)
    {
        StartCoroutine(Shaking(intensity, time));
    }

    private IEnumerator Shaking(float intensity, float time)
    {
        float scaledIntensity = Mathf.Log10(intensity * (CVCamera.m_Lens.OrthographicSize / defaultCameraSize));
        float timeLeft = time;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            cinemachineBasicMultiChannel.m_AmplitudeGain = Mathf.Lerp(0f, scaledIntensity, timeLeft / time);
            yield return new WaitForFixedUpdate();
        }

        cinemachineBasicMultiChannel.m_AmplitudeGain = 0f;
        yield return null;
    }
}
