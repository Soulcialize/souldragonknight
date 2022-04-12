using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AudioManagerSynced : MonoBehaviour
{
    private static AudioManagerSynced _instance;
    public static AudioManagerSynced Instance { get => _instance; }

    [SerializeField] private PhotonView photonView;

    private void Awake()
    {
        // singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Sets the volume of the main mix to the given volume.
    /// </summary>
    /// <param name="isLocalIncluded">Whether to play the sound locally.</param>
    /// <param name="toVolume">The target volume.</param>
    /// <param name="doFade">Whether to fade towards the target volume.</param>
    /// <param name="fadeLength">Duration of the fade. Only applicable if <c>doFade</c> is <c>true</c>.</param>
    public void SetAudioVolume(bool isLocalIncluded, float toVolume, bool doFade = false, float fadeLength = 3f)
    {
        photonView.RPC("RPC_SetAudioVolume", isLocalIncluded ? RpcTarget.All : RpcTarget.Others, toVolume, doFade, fadeLength);
    }

    [PunRPC]
    private void RPC_SetAudioVolume(float toVolume, bool doFade, float fadeLength)
    {
        AudioManager.Instance.SetAudioVolume(toVolume, doFade, fadeLength);
    }

    /// <summary>
    /// Plays the sound FX with the given name.
    /// </summary>
    /// <param name="isLocalIncluded">Whether to play the sound locally.</param>
    /// <param name="index">The index of the sound FX to play.</param>
    /// <param name="doFade">If true, fades the sound FX in. Else, it starts at default volume.</param>
    /// <param name="fadeLength">The length of the fade in.</param>
    public void PlaySoundFx(bool isLocalIncluded, SoundFx.LibraryIndex index, bool doFade = false, float fadeLength = 1f)
    {
        photonView.RPC("RPC_PlaySoundFx", isLocalIncluded ? RpcTarget.All : RpcTarget.Others, (byte)index, doFade, fadeLength);
    }

    [PunRPC]
    private void RPC_PlaySoundFx(byte index, bool doFade, float fadeLength)
    {
        AudioManager.Instance.PlaySoundFx((SoundFx.LibraryIndex)index, doFade, fadeLength);
    }

    /// <summary>
    /// Stops the sound FX with the given name.
    /// </summary>
    /// <param name="isLocalIncluded">Whether to play the sound locally.</param>
    /// <param name="index">The index of the sound FX to stop.</param>
    /// <param name="doFade">If true, fades the sound FX out before stopping. Else, it simply stops.</param>
    /// <param name="fadeLength">The length of the fade out.</param>
    public void StopSoundFx(bool isLocalIncluded, SoundFx.LibraryIndex index, bool doFade = false, float fadeLength = 1f)
    {
        photonView.RPC("RPC_StopSoundFx", isLocalIncluded ? RpcTarget.All : RpcTarget.Others, (byte)index, doFade, fadeLength);
    }

    [PunRPC]
    private void RPC_StopSoundFx(byte index, bool doFade, float fadeLength)
    {
        AudioManager.Instance.StopSoundFx((SoundFx.LibraryIndex)index, doFade, fadeLength);
    }

    /// <summary>
    /// Plays the music with the given name.
    /// </summary>
    /// <param name="isLocalIncluded">Whether to play the sound locally.</param>
    /// <param name="index">The index of the music to play.</param>
    /// <param name="doFade">If true, fades the music in. Else, music starts at default volume.</param>
    /// <param name="fadeLength">The length of the fade in.</param>
    public void PlayMusic(bool isLocalIncluded, Music.LibraryIndex index, bool doFade = false, float fadeLength = 1f)
    {
        photonView.RPC("RPC_PlayMusic", isLocalIncluded ? RpcTarget.All : RpcTarget.Others, (byte)index, doFade, fadeLength);
    }

    [PunRPC]
    private void RPC_PlayMusic(byte index, bool doFade, float fadeLength)
    {
        AudioManager.Instance.PlayMusic((Music.LibraryIndex)index, doFade, fadeLength);
    }

    /// <summary>
    /// Pauses the given music.
    /// </summary>
    /// <param name="isLocalIncluded">Whether to play the sound locally.</param>
    /// <param name="index">The index of the music to pause.</param>
    /// <param name="doFade">If true, fades the music out before pausing. Else, music simply pauses.</param>
    /// <param name="fadeLength">The length of the fade out.</param>
    public void PauseMusic(bool isLocalIncluded, Music.LibraryIndex index, bool doFade = false, float fadeLength = 1f)
    {
        photonView.RPC("RPC_PauseMusic", isLocalIncluded ? RpcTarget.All : RpcTarget.Others, (byte)index, doFade, fadeLength);
    }

    [PunRPC]
    private void RPC_PauseMusic(byte index, bool doFade, float fadeLength)
    {
        AudioManager.Instance.PauseMusic((Music.LibraryIndex)index, doFade, fadeLength);
    }

    /// <summary>
    /// Stops the given music.
    /// </summary>
    /// <param name="isLocalIncluded">Whether to play the sound locally.</param>
    /// <param name="index">The index of the music to stop.</param>
    /// <param name="doFade">If true, fades the music out before stopping. Else, music simply stops.</param>
    /// <param name="fadeLength">The length of the fade out.</param>
    public void StopMusic(bool isLocalIncluded, Music.LibraryIndex index, bool doFade = false, float fadeLength = 1f)
    {
        photonView.RPC("RPC_StopMusic", isLocalIncluded ? RpcTarget.All : RpcTarget.Others, (byte)index, doFade, fadeLength);
    }

    [PunRPC]
    private void RPC_StopMusic(byte index, bool doFade, float fadeLength)
    {
        AudioManager.Instance.StopMusic((Music.LibraryIndex)index, doFade, fadeLength);
    }
}
