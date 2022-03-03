using System;
using System.Collections;
using System.Collections.Generic;

namespace StateMachines
{
    public abstract class StateMachine
    {
        protected Dictionary<Type, HashSet<Type>> transitions = new Dictionary<Type, HashSet<Type>>();

        public State CurrState { get; private set; }

        private bool CanTransitionTo(State newState)
        {
            if (CurrState == null)
            {
                return true;
            }

            // TODO: clean this up
            foreach (Type fromStateType in transitions.Keys)
            {
                if (fromStateType.IsAssignableFrom(CurrState.GetType()))
                {
                    foreach (Type toStateType in transitions[fromStateType])
                    {
                        if (toStateType.IsAssignableFrom(newState.GetType()))
                        {
                            return true;
                        }
                    }

                    break;
                }
            }

            return false;
        }

        public void ChangeState(State newState)
        {
            if (CanTransitionTo(newState))
            {
                CurrState?.Exit();
                CurrState = newState;
                CurrState.Enter();
            }
        }

        public virtual void Update()
        {
            CurrState?.Execute();
        }

        public void Exit()
        {
            CurrState?.Exit();
            CurrState = null;
        }
    }
}
