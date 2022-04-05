using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonFx : MonoBehaviour
{
    public void Play()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.MENU_BUTTON);
    }
}
