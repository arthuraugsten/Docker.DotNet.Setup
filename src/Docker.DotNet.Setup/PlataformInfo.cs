using Docker.DotNet.Setup.Abstractions;
using System.Runtime.InteropServices;

namespace Docker.DotNet.Setup
{
    public sealed class PlataformInfo : IPlataformInfo
    {
        private static readonly bool _isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private static readonly bool _isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public bool IsWindows => _isWindows;
        public bool IsLinux => _isLinux;
    }
}