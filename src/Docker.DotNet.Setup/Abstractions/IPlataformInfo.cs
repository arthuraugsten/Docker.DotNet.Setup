namespace Docker.DotNet.Setup.Abstractions
{
    public interface IPlataformInfo
    {
        bool IsWindows { get; }
        bool IsLinux { get; }
    }
}