using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : ActorController
{
    public BehaviorTreesManager BehaviorTreesManager { get; private set; }

    protected virtual void Start()
    {
        BehaviorTreesManager = InitializeBehaviorTreesManager();
    }

    protected override void Update()
    {
        base.Update();
        if (photonView.IsMine)
        {
            BehaviorTreesManager.UpdateActiveTree();
        }
    }

    protected abstract BehaviorTreesManager InitializeBehaviorTreesManager();
}
