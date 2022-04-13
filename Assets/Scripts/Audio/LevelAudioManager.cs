using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudioManager : MonoBehaviour
{
    private static LevelAudioManager _instance;
    public static LevelAudioManager Instance { get => _instance; }

    public Music.LibraryIndex LastPlayedMusic { get; private set; }

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

    void Start()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.LEVEL_START);
        PlayLevelDefaultMusic(false);
    }

    public void StopLastPlayedMusic(bool isSynced)
    {
        if (isSynced)
        {
            AudioManagerSynced.Instance.StopMusic(true, LastPlayedMusic, true);
        }
        else
        {
            AudioManager.Instance.StopMusic(LastPlayedMusic, true);
        }
    }

    public void PlayLevelDefaultMusic(bool isSynced)
    {
        LastPlayedMusic = Music.LibraryIndex.INGAME_BACKGROUND_MUSIC;
        if (isSynced)
        {
            AudioManagerSynced.Instance.PlayMusic(true, Music.LibraryIndex.INGAME_BACKGROUND_MUSIC, true, 0.1f);
        }
        else
        {
            AudioManager.Instance.PlayMusic(Music.LibraryIndex.INGAME_BACKGROUND_MUSIC, true, 0.1f);
        }
    }

    public void StopLevelDefaultMusic(bool isSynced)
    {
        if (isSynced)
        {
            AudioManagerSynced.Instance.StopMusic(true, Music.LibraryIndex.INGAME_BACKGROUND_MUSIC, true, 5f);
        }
        else
        {
            AudioManager.Instance.StopMusic(Music.LibraryIndex.INGAME_BACKGROUND_MUSIC, true, 5f);
        }
    }
    public void PlayLevelPuzzleMusic(bool isSynced)
    {
        LastPlayedMusic = Music.LibraryIndex.PUZZLE_MUSIC;
        if (isSynced)
        {
            AudioManagerSynced.Instance.PlayMusic(true, Music.LibraryIndex.PUZZLE_MUSIC, true, 3f);
        }
        else
        {
            AudioManager.Instance.PlayMusic(Music.LibraryIndex.PUZZLE_MUSIC, true, 3f);
        }
    }

    public void StopLevelPuzzleMusic(bool isSynced)
    {
        if (isSynced)
        {
            AudioManagerSynced.Instance.StopMusic(true, Music.LibraryIndex.PUZZLE_MUSIC, true, 3f);
        }
        else
        {
            AudioManager.Instance.StopMusic(Music.LibraryIndex.PUZZLE_MUSIC, true, 3f);
        }
    }

    public void PlayLevelFinaleMusic(bool isSynced)
    {
        LastPlayedMusic = Music.LibraryIndex.FINALE_MUSIC;
        if (isSynced)
        {
            AudioManagerSynced.Instance.PlayMusic(true, Music.LibraryIndex.FINALE_MUSIC, true, 0.1f);
        }
        else
        {
            AudioManager.Instance.PlayMusic(Music.LibraryIndex.FINALE_MUSIC, true, 0.1f);
        }
    }

    public void StopLevelFinaleMusic(bool isSynced)
    {
        if (isSynced)
        {
            AudioManagerSynced.Instance.StopMusic(true, Music.LibraryIndex.INGAME_BACKGROUND_MUSIC, true, 3f);
        }
        else
        {
            AudioManager.Instance.StopMusic(Music.LibraryIndex.INGAME_BACKGROUND_MUSIC, true, 3f);
        }
    }

    public void PlayLevelVictoryMusic(bool isSynced)
    {
        LastPlayedMusic = Music.LibraryIndex.VICTORY_MUSIC;
        if (isSynced)
        {
            AudioManagerSynced.Instance.PlayMusic(true, Music.LibraryIndex.VICTORY_MUSIC, true, 0.1f);
        }
        else
        {
            AudioManager.Instance.PlayMusic(Music.LibraryIndex.VICTORY_MUSIC, true, 0.1f);
        }
    }
}
