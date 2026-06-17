namespace LLib
{
    public abstract class BaseState<T> : IState
    {
        protected readonly T Owner;
        protected BaseState(T owner) => Owner = owner;
 
        public virtual void OnEnter(){ }
        public virtual void OnUpdate(){ }
        public virtual void OnFixedUpdate(){ }
        public virtual void OnExit(){ }
    }
}

