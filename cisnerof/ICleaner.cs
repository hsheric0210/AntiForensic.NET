namespace cisnerof
{
    internal interface ICleaner
    {
        string Name { get; }
        int RunCleaner();
    }
}
