namespace Docker.DotNet.Setup.Abstractions
{
    internal interface IPlataformInfo
    {
        bool IsWindows { get; }
        bool IsLinux { get; }
    }
}