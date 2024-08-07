using System;
using System.Collections.Generic;
using System.Linq;
using Common.MVC.Data;
using JetBrains.Annotations;
using UnityEngine;

namespace Common.MVC.View
{
    public abstract class UiDataView<T> : MonoBehaviour, IView, IData<T>
    {
        public T Data { get; private set; }

        private readonly Dictionary<Type, List<IController>> _controllers = new();
        
        private bool _isInitialized;
        
        public virtual void SetControllers(params IController[] controllers)
        {
            _controllers.Clear();
            if(controllers == null) return;
            foreach (var controller in controllers)
            {
                var controllerType = controller.GetType();

                if (_controllers.ContainsKey(controllerType))
                {
                    _controllers[controllerType].Add(controller);
                    continue;
                }

                _controllers[controllerType] = new List<IController> { controller };
            }
        }

        protected void Interact<TController>(Action<TController> action)
            where TController : IController
        {
            if (!_controllers.TryGetValue(typeof(TController), out var controllersConcrete)) return;
            foreach (var controller in controllersConcrete)
            {
                action.Invoke((TController)controller);
            }
        }

        public void SetData(T data)
        {
            var dataPrevious = Data;
            Data = data;
            OnSetData(dataPrevious, Data);
        }
        
        public virtual void Show()
        {
            if (_isInitialized == false)
            {
                Init();
                _isInitialized = true;
            }
            
            if(gameObject == null) return;
            gameObject.SetActive(true);
            Refresh();
        }
        
        protected abstract void Init();

        public abstract void Refresh();
        
        public virtual void Hide()
        {
            if (gameObject == null) return;
                gameObject.SetActive(false);
        }
        
        public virtual void HideAfterAnimationEvent()
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }

        protected virtual void OnSetData([CanBeNull] T dataPrevious, [NotNull] T dataCurrent)
        {
        }

        protected virtual bool IsControllerPresent<V>() where V : IController
        {
            return _controllers.ContainsKey(typeof(V));
        }

        public virtual bool TryGetController<V>(out V controller) where V : IController
        {
            controller = default;

            if (!_controllers.TryGetValue(typeof(V), out var controllers)) return false;
            controller = (V) controllers.First();
            return true;

        }
    }
}