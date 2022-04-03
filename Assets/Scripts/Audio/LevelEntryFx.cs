using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntryFx : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySoundFx(SoundFx.LibraryIndex.LEVEL_START);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
