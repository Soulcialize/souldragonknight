using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTrees;

public class BehaviorTreesManager
{
    public BehaviorTree.Function ActiveTree { get; private set; }

    private Dictionary<BehaviorTree.Function, BehaviorTree> functionToTreesDictionary;

    public BehaviorTreesManager(
        Dictionary<BehaviorTree.Function, BehaviorTree> functionToTreesDictionary,
        BehaviorTree.Function startTree)
    {
        this.functionToTreesDictionary = functionToTreesDictionary;

        ActiveTree = startTree;
        functionToTreesDictionary[ActiveTree].Enter();
    }

    public void UpdateActiveTree()
    {
        if (ActiveTree != BehaviorTree.Function.NONE)
        {
            functionToTreesDictionary[ActiveTree].Update();
        }
    }

    public void SwitchActiveTree(BehaviorTree.Function newTree)
    {
        if (ActiveTree != BehaviorTree.Function.NONE)
        {
            functionToTreesDictionary[ActiveTree].Exit();
        }

        ActiveTree = newTree;

        if (ActiveTree != BehaviorTree.Function.NONE)
        {
            functionToTreesDictionary[ActiveTree].Enter();
        }
    }
}
