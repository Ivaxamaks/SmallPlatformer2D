public class LastGameResultProvider
{
    public bool IsWin { get; private set; }

    public void SetLastResult(bool isWin)
    {
        IsWin = isWin;
    }
}