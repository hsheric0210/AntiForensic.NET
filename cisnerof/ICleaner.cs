namespace cisnerof
{
    internal interface ICleaner
    {
        CleanerTypes Type { get; }
        string Name { get; }
        int RunCleaner();
    }
}
