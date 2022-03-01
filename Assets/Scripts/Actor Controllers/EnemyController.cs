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

    protected virtual void Update()
    {
        BehaviorTreesManager.UpdateActiveTree();
    }

    protected abstract BehaviorTreesManager InitializeBehaviorTreesManager();
}
