using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    public abstract Movement Movement { get; }
    public abstract Combat Combat { get; }
}
