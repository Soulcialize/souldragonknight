using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXIndexLibrary : MonoBehaviour
{
    [SerializeField] private SoundFx.LibraryIndex attack;
    [SerializeField] private SoundFx.LibraryIndex dodge;
    [SerializeField] private SoundFx.LibraryIndex die;
    [SerializeField] private SoundFx.LibraryIndex hurt;
    [SerializeField] private SoundFx.LibraryIndex blockhit;
    [SerializeField] private SoundFx.LibraryIndex revive;
    [SerializeField] private SoundFx.LibraryIndex readyattack;


    public SoundFx.LibraryIndex Attack { get => attack; }
    public SoundFx.LibraryIndex Dodge { get => dodge; }
    public SoundFx.LibraryIndex Die { get => die; }
    public SoundFx.LibraryIndex Hurt { get => hurt; }
    public SoundFx.LibraryIndex BlockHit { get => blockhit; }
    public SoundFx.LibraryIndex Revive { get => revive; }
    public SoundFx.LibraryIndex ReadyAttack { get => readyattack; }

}
