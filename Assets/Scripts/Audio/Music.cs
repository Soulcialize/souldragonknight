using UnityEngine;

[System.Serializable]
public class Music : Sound
{
    public enum LibraryIndex
    {
        MENU_BACKGROUND_MUSIC,
        INGAME_BACKGROUND_MUSIC,
        DEFEAT_MUSIC
    }

    [Space(10)]

    [SerializeField] private LibraryIndex libraryIndex;
    [SerializeField] private AudioClip audioClip;

    public LibraryIndex Index { get => libraryIndex; }

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
