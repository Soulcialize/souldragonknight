using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static readonly float MAIN_VOLUME_MIN = -80f;
    public static readonly float MAIN_VOLUME_MAX = 0f;

    private static AudioManager _instance;
    public static AudioManager Instance { get => _instance; }

    [SerializeField] private AudioMixer mainMix;
    [SerializeField] private string mainMixVolumeParam = "MasterVolume";

    [Space(10)]

    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup musicGroup;

    [Space(10)]

    [SerializeField] private SoundFx[] soundFxs;
    [SerializeField] private Music[] musicTracks;

    private Dictionary<SoundFx.LibraryIndex, SoundFx> soundFxLibrary;
    private Dictionary<Music.LibraryIndex, Music> musicLibrary;

    void Awake()
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

        soundFxLibrary = new Dictionary<SoundFx.LibraryIndex, SoundFx>();
        foreach (SoundFx soundFx in soundFxs)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = sfxGroup;
            soundFx.InitialiseSound(audioSource);
            soundFxLibrary[soundFx.Index] = soundFx;
        }

        musicLibrary = new Dictionary<Music.LibraryIndex, Music>();
        foreach (Music music in musicTracks)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = musicGroup;
            music.InitialiseSound(audioSource);
            musicLibrary[music.Index] = music;
        }
    }

    #region Public Methods - Audio
    /// <summary>
    /// Sets the volume of the main mix to the given volume.
    /// </summary>
    /// <param name="toVolume">The target volume.</param>
    /// <param name="doFade">Whether to fade towards the target volume.</param>
    /// <param name="fadeLength">Duration of the fade. Only applicable if <c>doFade</c> is <c>true</c>.</param>
    public void SetAudioVolume(float toVolume, bool doFade = false, float fadeLength = 3f)
    {
        if (!doFade)
        {
            mainMix.SetFloat(mainMixVolumeParam, toVolume);
        }
        else
        {
            StartCoroutine(FadeAllAudio(toVolume, fadeLength));
        }
    }
    #endregion

    #region Public Methods - SFX
    /// <summary>
    /// Plays the sound FX with the given name.
    /// </summary>
    /// <param name="index">The index of the sound FX to play.</param>
    /// <param name="doFade">If true, fades the sound FX in. Else, it starts at default volume.</param>
    /// <param name="fadeTime">The length of the fade in.</param>
    public void PlaySoundFx(SoundFx.LibraryIndex index, bool doFade = false, float fadeTime = 1f)
    {
        if (!soundFxLibrary.ContainsKey(index))
        {
            Debug.LogError($"SoundFx [{index}] not found in AudioManager.");
            return;
        }

        SoundFx soundFx = soundFxLibrary[index];
        soundFx.Play();

        if (doFade)
        {
            StartCoroutine(FadeSound(soundFx, 0, soundFx.DefaultVolume, fadeTime));
        }
        else
        {
            soundFx.Volume = soundFx.DefaultVolume;
        }
    }

    /// <summary>
    /// Stops the sound FX with the given name.
    /// </summary>
    /// <param name="index">The index of the sound FX to stop.</param>
    /// <param name="doFade">If true, fades the sound FX out before stopping. Else, it simply stops.</param>
    /// <param name="fadeTime">The length of the fade out.</param>
    public void StopSoundFx(SoundFx.LibraryIndex index, bool doFade = false, float fadeTime = 1f)
    {
        if (!soundFxLibrary.ContainsKey(index))
        {
            Debug.LogError($"SoundFx [{index}] not found in AudioManager.");
            return;
        }

        SoundFx soundFx = soundFxLibrary[index];
        if (doFade)
        {
            StartCoroutine(FadeSound(soundFx, soundFx.Volume, 0, fadeTime, soundFx.Stop));
        }
        else
        {
            soundFx.Stop();
        }
    }
    #endregion

    #region Public Methods - Music
    /// <summary>
    /// Plays the music with the given name.
    /// </summary>
    /// <param name="index">The index of the music to play.</param>
    /// <param name="doFade">If true, fades the music in. Else, music starts at default volume.</param>
    /// <param name="fadeTime">The length of the fade in.</param>
    public void PlayMusic(Music.LibraryIndex index, bool doFade = false, float fadeTime = 1f)
    {
        if (!musicLibrary.ContainsKey(index))
        {
            Debug.LogError($"Music [{index}] not found in AudioManager.");
            return;
        }

        Music music = musicLibrary[index];
        music.Play();

        if (doFade)
        {
            StartCoroutine(FadeSound(music, 0, music.DefaultVolume, fadeTime));
        }
        else
        {
            music.Volume = music.DefaultVolume;
        }
    }

    /// <summary>
    /// Pauses the given music.
    /// </summary>
    /// <param name="index">The index of the music to pause.</param>
    /// <param name="doFade">If true, fades the music out before pausing. Else, music simply pauses.</param>
    /// <param name="fadeTime">The length of the fade out.</param>
    public void PauseMusic(Music.LibraryIndex index, bool doFade = false, float fadeTime = 1f)
    {
        if (!musicLibrary.ContainsKey(index))
        {
            Debug.LogError($"Music [{index}] not found in AudioManager.");
            return;
        }

        Music music = musicLibrary[index];
        if (doFade)
        {
            StartCoroutine(FadeSound(music, music.Volume, 0, fadeTime, () => PauseMusic(index)));
        }
        else
        {
            music.Pause();
        }
    }

    /// <summary>
    /// Stops the given music.
    /// </summary>
    /// <param name="index">The index of the music to stop.</param>
    /// <param name="doFade">If true, fades the music out before stopping. Else, music simply stops.</param>
    /// <param name="fadeTime">The length of the fade out.</param>
    public void StopMusic(Music.LibraryIndex index, bool doFade = false, float fadeTime = 1f)
    {
        if (!musicLibrary.ContainsKey(index))
        {
            Debug.LogError($"Music [{index}] not found in AudioManager.");
            return;
        }

        Music music = musicLibrary[index];
        if (doFade)
        {
            StartCoroutine(FadeSound(music, music.Volume, 0, fadeTime, music.Stop));
        }
        else
        {
            music.Stop();
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Fades all audio from the current mix volume to the given volume.
    /// </summary>
    /// <param name="toVolume">The target volume in decibels.</param>
    /// <param name="fadeLength">The length of the fade.</param>
    /// <returns>IEnumerator; this is a coroutine.</returns>
    private IEnumerator FadeAllAudio(float toVolume, float fadeLength)
    {
        mainMix.GetFloat(mainMixVolumeParam, out float fromVolume);

        float elapsedTime = 0f;
        while (elapsedTime < fadeLength)
        {
            // linear volume change
            mainMix.SetFloat(mainMixVolumeParam, Mathf.Lerp(fromVolume, toVolume, elapsedTime / fadeLength));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainMix.SetFloat(mainMixVolumeParam, toVolume);
    }

    /// <summary>
    /// Fades the volume of the given sound between the two given volume levels.
    /// </summary>
    /// <param name="sound">The sound whose volume to fade.</param>
    /// <param name="fromVolume">The volume from which to start.</param>
    /// <param name="toVolume">The target volume.</param>
    /// <param name="fadeLength">The duration of the fade.</param>
    /// <param name="endCallback">A callback that is invoked after the fade is complete.</param>
    /// <returns>IEnumerator; this is a coroutine.</returns>
    private IEnumerator FadeSound(Sound sound, float fromVolume, float toVolume, float fadeLength, UnityAction endCallback = null)
    {
        sound.Volume = fromVolume;

        float elapsedTime = 0f;
        while (elapsedTime < fadeLength)
        {
            sound.Volume = Mathf.Lerp(fromVolume, toVolume, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sound.Volume = toVolume;
        endCallback?.Invoke();
    }
    #endregion
}
