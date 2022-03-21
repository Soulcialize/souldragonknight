using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : ActorController
{
    [Tooltip("This toggle is for allowing enemy visibility during debugging only.")]
    [SerializeField] private bool hideVisibility;
    [SerializeField] protected Visibility visibility;
    [SerializeField] protected float hurtRevealDuration;

    public BehaviorTreesManager BehaviorTreesManager { get; private set; }

    protected virtual void Start()
    {
        if (photonView.IsMine)
        {
            if (hideVisibility)
            {
                visibility.Hide();
            }

            BehaviorTreesManager = InitializeBehaviorTreesManager();
        }
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

    protected override void HandleHurtEvent()
    {
        base.HandleHurtEvent();
        if (hideVisibility)
        {
            visibility.RevealBriefly(hurtRevealDuration);
        }
    }

    protected override void HandleDeathEvent()
    {
        base.HandleDeathEvent();
        BehaviorTreesManager.SwitchActiveTree(AiBehaviorTrees.BehaviorTree.Function.NONE);
        visibility.Reveal();
    }
}
