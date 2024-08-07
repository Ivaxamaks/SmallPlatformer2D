namespace Events
{
    public struct EnemySpawnEvent 
    {
        public readonly Enemy.Enemy Enemy;

        public EnemySpawnEvent(Enemy.Enemy enemy)
        {
            Enemy = enemy;
        }
    }
}