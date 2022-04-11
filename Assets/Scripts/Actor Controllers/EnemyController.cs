using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : ActorController
{
    [Tooltip("This toggle is for allowing enemy visibility during debugging only.")]
    [SerializeField] protected bool hideVisibility;
    [SerializeField] protected Detection detection;
    [SerializeField] protected Visibility visibility;
    [SerializeField] protected float hurtRevealDuration;

    public Detection Detection { get => detection; }

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
        visibility.RevealBriefly(hurtRevealDuration);
    }

    protected override void HandleDeathEvent()
    {
        base.HandleDeathEvent();
        BehaviorTreesManager?.SwitchActiveTree(AiBehaviorTrees.BehaviorTree.Function.NONE);
        pathfinder.StopPathfind();
        visibility.Reveal();
    }
}
