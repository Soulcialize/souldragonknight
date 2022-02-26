namespace StateMachines
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }
}
