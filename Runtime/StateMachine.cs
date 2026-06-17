using System;
using System.Collections.Generic;

namespace LLib
{
    public class StateMachine
    {
        private class Transition
        {
            public IState TargetState { get; }
            private readonly Func<bool> _condition;
            public Transition(IState target, Func<bool> condition)
            {
                TargetState = target;
                _condition  = condition;
            }
            public bool Condition() => _condition();
        }

        private readonly Dictionary<Type, List<Transition>> _transitions = new();
        private readonly List<Transition> _anyTransitions = new();

        public IState CurrentState { get; private set; }

        // ( form, to )
        public event Action<IState, IState> OnStateChanged;

        public void SetState(IState state)
        {
            if (state == CurrentState) 
                return;
            
            CurrentState?.OnExit();
            
            var prev = CurrentState;
            CurrentState = state;
            
            CurrentState.OnEnter();
            
            OnStateChanged?.Invoke(prev, CurrentState);
        }

        
        public void Update()
        {
            var transition = GetTriggeredTransition();
            if (transition != null)
                SetState(transition.TargetState);

            CurrentState?.OnUpdate();
        }

        
        public void FixedUpdate()
        {
            CurrentState?.OnFixedUpdate();
        }

        
        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            var type = from.GetType();
            if (!_transitions.TryGetValue(type, out var list))
            {
                list = new List<Transition>();
                _transitions[type] = list;
            }
            list.Add(new Transition(to, condition));
        }

        
        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            _anyTransitions.Add(new Transition(to, condition));
        }


        private Transition GetTriggeredTransition()
        {
            foreach (var t in _anyTransitions)
            {
                if (t.Condition()) 
                    return t;
            }
                
            var list = _transitions.GetValueOrDefault(CurrentState.GetType());
            if (list == null)
                return null;
            
            
            foreach (var t in list)
                if (t.Condition()) return t;

            return null;
        }
    }
}