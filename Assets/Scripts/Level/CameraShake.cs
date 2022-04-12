using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake _instance;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    public static CameraShake Instance { get => _instance; }

    private void Awake()
    {
        _instance = this;
        cinemachineBasicMultiChannel = cinemachineVirtualCamera
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float intensity, float time)
    {
        StartCoroutine(Shaking(intensity, time));
    }

    private IEnumerator Shaking(float intensity, float time)
    {
        float timeLeft = time;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            cinemachineBasicMultiChannel.m_AmplitudeGain = Mathf.Lerp(0f, intensity, timeLeft / time);
            yield return new WaitForFixedUpdate();
        }

        cinemachineBasicMultiChannel.m_AmplitudeGain = 0f;
        yield return null;
    }
}
