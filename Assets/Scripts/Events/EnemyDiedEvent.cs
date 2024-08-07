namespace Events
{
    public struct EnemyDiedEvent
    {
        public readonly Enemy.Enemy Enemy;

        public EnemyDiedEvent(Enemy.Enemy enemy)
        {
            Enemy = enemy;
        }
    }
}