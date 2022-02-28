using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    [SerializeField] protected Combat combat;

    public abstract Movement Movement { get; }
    public Combat Combat { get => combat; }
}
