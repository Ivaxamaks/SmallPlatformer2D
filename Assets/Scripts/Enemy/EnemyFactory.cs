using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Enemy
{
    public class EnemyFactory
    {
        private readonly IObjectResolver _container;
        private readonly Enemy _enemy;

        public EnemyFactory(IObjectResolver container,
            Enemy enemy)
        {
            _container = container;
            _enemy = enemy;
        }

        public Enemy CreateEnemy(Transform transform)
        { 
            var enemy = _container.Instantiate(_enemy);
            return enemy;
        }
    }
}