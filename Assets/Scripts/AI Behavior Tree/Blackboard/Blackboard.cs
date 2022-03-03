using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AiBehaviorTreeNodes;

namespace AiBehaviorTreeBlackboards
{
    public class Blackboard
    {
        private Dictionary<string, object> dataContext = new Dictionary<string, object>();

        public static void CreateBlackboardForTree(BehaviorNode root)
        {
            SetBlackboardForTreeNodes(root, new Blackboard());
        }

        private static void SetBlackboardForTreeNodes(BehaviorNode node, Blackboard blackboard)
        {
            node.Blackboard = blackboard;
            if (node is CompositeNode compositeNode)
            {
                foreach (BehaviorNode child in compositeNode.Children)
                {
                    SetBlackboardForTreeNodes(child, blackboard);
                }
            }
            else if (node is DecoratorNode decoratorNode)
            {
                SetBlackboardForTreeNodes(decoratorNode.Child, blackboard);
            }
        }

        public object GetData(string key, object defaultValue = null)
        {
            if (dataContext.TryGetValue(key, out object data))
            {
                return data;
            }

            dataContext[key] = defaultValue;
            return defaultValue;
        }

        public void SetData(string key, object value)
        {
            dataContext[key] = value;
        }

        public void ClearData(string key)
        {
            dataContext.Remove(key);
        }

        public void ClearAllData()
        {
            dataContext.Clear();
        }
    }
}
