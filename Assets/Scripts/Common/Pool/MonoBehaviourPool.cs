using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Pool
{
    public class MonoBehaviourPool<T> where T : Component
    {
        public ReadOnlyCollection<T> UsedItems { get; private set; }

        private readonly List<T> _notUsedItems = new();
        private readonly List<T> _usedItems = new();

        private readonly Transform _parent;
        private readonly T _prefab;
        private readonly Func<Transform, T> _factoryMethod;

        public MonoBehaviourPool(T prefab, Transform parent, int defaultCount = 4)
        {
            _parent = parent;
            _prefab = prefab;
            WarmUp(defaultCount);
        }

        public MonoBehaviourPool(Func<Transform, T> factoryMethod, Transform parent, int defaultCount = 4)
        {
            _parent = parent;
            _factoryMethod = factoryMethod;
            WarmUp(defaultCount);
        }

        private void WarmUp(int defaultCount)
        {
            for (int i = 0; i < defaultCount; i++)
            {
                AddNewItemInPool();
            }

            SortBySiblingIndexUnused();
            UsedItems = new ReadOnlyCollection<T>(_usedItems);
        }

        public T Take()
        {
            if (_notUsedItems.Count == 0)
            {
                AddNewItemInPool();
            }

            var lastIndex = _notUsedItems.Count - 1;
            var itemFromPool = _notUsedItems[lastIndex];
            _notUsedItems.RemoveAt(lastIndex);
            _usedItems.Add(itemFromPool);
            itemFromPool.gameObject.SetActive(true);

            return itemFromPool;
        }

        public void ReleaseAll()
        {
            foreach (var item in _usedItems)
            {
                if(item == null || item.gameObject == null) continue;
                    item.gameObject.SetActive(false);
            }
            
            _notUsedItems.AddRange(_usedItems);
            _usedItems.Clear();
            _notUsedItems.RemoveAll(item => item == null || item.gameObject == null);
            
            SortBySiblingIndexUnused();
        }

        private void SortBySiblingIndexUnused()
        {
            _notUsedItems.Sort((a, b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));
        }

        public void Release(T item)
        {
            item.gameObject.SetActive(false);
            _usedItems.Remove(item);
            _notUsedItems.Add(item);
        }

        public void Release(List<T> items)
        {
            foreach (var item in items)
            {
                Release(item);
            }
        }

        protected virtual T Instantiate()
        {
            return _factoryMethod != null
                ? _factoryMethod(_parent)
                : Object.Instantiate(_prefab, _parent, false);
        }

        private void AddNewItemInPool()
        {
            var newItem = Instantiate();
            newItem.gameObject.SetActive(false);

            var returnToPoolOnDisable = newItem.GetComponent<ReturnToPoolOnDisable>();
            if (returnToPoolOnDisable != null)
            {
                returnToPoolOnDisable.Initialize(gameObject => Release(newItem));
            }

            _notUsedItems.Add(newItem);
        }
    }
}