using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(Music.LibraryIndex.MENU_BACKGROUND_MUSIC);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
