namespace AntiForensicLib
{
    public interface ICleaner
    {
        CleanerTypes Type { get; }
        string Name { get; }
        int RunCleaner();
    }
}
