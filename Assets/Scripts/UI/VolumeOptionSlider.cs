using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;

public enum AudioGroup { MAIN, MUSIC, SFX };

public class VolumeOptionSlider : OptionSlider
{
    public static readonly float VOLUME_MIN = 0f;
    public static readonly float VOLUME_MAX = 100f;

    [SerializeField] private AudioGroup audioGroup;

    private void Start()
    {
        var volume = audioGroup switch
        {
            AudioGroup.MAIN => AudioManager.Instance.GetAudioVolume(),
            AudioGroup.MUSIC => AudioManager.Instance.GetMusicVolume(),
            AudioGroup.SFX => AudioManager.Instance.GetSfxVolume(),
            _ => throw new System.ArgumentException("Unknown audio group"),
        };
        float toValue = ConvertToValue(volume);

        slider.value = toValue;
    }

    protected override void UpdateValue(float value)
    {
        base.UpdateValue(value);
        float toVolume = ConvertToVolume(value);

        switch (audioGroup)
        {
            case AudioGroup.MAIN:
                AudioManager.Instance.SetAudioVolume(toVolume);
                break;

            case AudioGroup.MUSIC:
                AudioManager.Instance.SetMusicVolume(toVolume);
                break;

            case AudioGroup.SFX:
                AudioManager.Instance.SetSfxVolume(toVolume);
                break;

            default:
                throw new System.ArgumentException("Unknown audio group");
        }
    }

    private float ConvertToValue(float volume)
    {
        return volume == AudioManager.MAIN_VOLUME_MIN 
            ? VOLUME_MIN 
            : Mathf.Pow(10, (volume / 40f)) * (VOLUME_MAX - VOLUME_MIN);
    }

    private float ConvertToVolume(float value)
    {
        return value == VOLUME_MIN 
            ? AudioManager.MAIN_VOLUME_MIN 
            : Mathf.Log10(value / (VOLUME_MAX - VOLUME_MIN)) * 40f;
    }
}
