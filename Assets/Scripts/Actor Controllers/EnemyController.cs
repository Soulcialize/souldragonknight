using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : ActorController
{
    [SerializeField] protected Visibility visibility;
    [SerializeField] protected float hurtRevealDuration;

    public BehaviorTreesManager BehaviorTreesManager { get; private set; }

    protected virtual void OnEnable()
    {
        Combat.HurtEvent.AddListener(HandleHurtEvent);
    }

    protected virtual void OnDisable()
    {
        Combat.HurtEvent.RemoveListener(HandleHurtEvent);
    }

    protected virtual void Start()
    {
        if (photonView.IsMine)
        {
            visibility.Hide();
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

    protected virtual void HandleHurtEvent()
    {
        visibility.RevealBriefly(hurtRevealDuration);
    }
}
