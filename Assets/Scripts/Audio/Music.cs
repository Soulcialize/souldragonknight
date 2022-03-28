using UnityEngine;

[System.Serializable]
public class Music : Sound
{
    [SerializeField] private AudioClip audioClip;

    /// <summary>
    /// Sets the sound's audio source to the given AudioSource.
    /// Also sets the source's volume and loop setting to the sound's default volume and loop setting.
    /// </summary>
    /// <remarks>The source's audio clip is set to the music's audio clip.</remarks>
    /// <param name="audioSource">The AudioSource to set this sound's audio source to.</param>
    public override void InitialiseSound(AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        base.InitialiseSound(audioSource);
    }

    /// <summary>
    /// Plays the music.
    /// </summary>
    public override void Play()
    {
        audioSource.Play();
    }
}
