using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundMusic : MonoBehaviour
{
    [SerializeField] private float fadeInTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(Music.LibraryIndex.MENU_BACKGROUND_MUSIC, true, fadeInTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
