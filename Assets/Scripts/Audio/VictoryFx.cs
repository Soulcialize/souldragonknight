using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryFx : MonoBehaviour
{
    public void Play()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.LEVEL_VICTORY);
    }
}
