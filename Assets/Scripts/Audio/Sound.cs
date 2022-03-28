using UnityEngine;

public abstract class Sound
{
    protected AudioSource audioSource;

    [SerializeField] protected string name;
    [Range(0, 1)]
    [SerializeField] protected float defaultVolume = 1f;
    [SerializeField] protected bool loop = false;

    public string Name { get => name; }
    public float DefaultVolume { get => defaultVolume; }
    public float Volume { get => audioSource.volume; set => audioSource.volume = value; }
    public bool Loop { get => audioSource.loop; set => audioSource.loop = value; }

    /// <summary>
    /// Sets the sound's audio source to the given AudioSource.
    /// Also sets the source's volume and loop setting to the sound's default volume and loop setting.
    /// </summary>
    /// <param name="audioSource">The AudioSource to set this sound's audio source to.</param>
    public virtual void InitialiseSound(AudioSource audioSource)
    {
        this.audioSource = audioSource;
        Volume = defaultVolume;
        Loop = loop;
    }

    /// <summary>
    /// Plays the sound.
    /// </summary>
    public abstract void Play();

    /// <summary>
    /// Pauses the sound.
    /// </summary>
    public virtual void Pause()
    {
        audioSource.Pause();
    }

    /// <summary>
    /// Stops the sound.
    /// </summary>
    public virtual void Stop()
    {
        audioSource.Stop();
    }
}
