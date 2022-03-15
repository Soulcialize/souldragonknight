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
        functionToTreesDictionary[ActiveTree].Update();
    }

    public void SwitchActiveTree(BehaviorTree.Function newTree)
    {
        functionToTreesDictionary[ActiveTree].Exit();
        ActiveTree = newTree;
        functionToTreesDictionary[ActiveTree].Enter();
    }
}
