using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.StateMachine
{
    public abstract class StateMachine : IStateMachine, IDisposable
    {
        public IExitableState CurrentState { get; private set; }
        private readonly Dictionary<Type, IExitableState> _states = new();

        protected StateMachine(params IExitableState[] states)
        {
            foreach (var state in states)
            {
                state.Initialize(this);
                _states.Add(state.GetType(), state);
            }
        }

        public async UniTask Enter<TState>() where TState : class, IState
        {
            var newState = await ChangeState<TState>();
            
            Debug.Log($"[StateMachine] Enter: {typeof(TState).Name}");
            await newState.Enter();
        }

        public async UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPaylodedState<TPayload>
        {
            var newState = await ChangeState<TState>();
            
            Debug.Log($"[StateMachine] Enter: {typeof(TState).Name}");
            await newState.Enter(payload);
        }

        private async UniTask<TState> ChangeState<TState>() where TState : class, IExitableState
        {
            if (CurrentState != null)
            {
                Debug.Log($"[StateMachine] Exit: {CurrentState.GetType().Name}");
                await CurrentState.Exit();
            }
      
            var state = GetState<TState>();
            CurrentState = state;
      
            return state;
        }
    
        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }

        public virtual void Dispose()
        {
            if (CurrentState == null)
            {
                return;
            }
            
            Debug.Log($"[StateMachine] Exit: {CurrentState.GetType().Name}");
            CurrentState.Exit();
        }
    }
}