using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    [SerializeField] protected Combat combat;

    public Combat Combat { get => combat; }
}
