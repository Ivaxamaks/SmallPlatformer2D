namespace Events
{
    public struct PlayerAmmoAmountChangedEvent
    {
        public readonly int AmmoCount;

        public PlayerAmmoAmountChangedEvent(int ammoCount)
        {
            AmmoCount = ammoCount;
        }
    }
}