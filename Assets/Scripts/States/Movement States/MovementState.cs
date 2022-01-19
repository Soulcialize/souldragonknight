using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementState : State
{
    protected readonly Movement owner;

    public MovementState(Movement owner)
    {
        this.owner = owner;
    }

    public override abstract void Enter();
    public override abstract void Execute();
    public override abstract void Exit();
}
