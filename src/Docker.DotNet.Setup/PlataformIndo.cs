using System.Runtime.InteropServices;
using Docker.DotNet.Setup.Abstractions;

namespace Docker.DotNet.Setup
{
    internal sealed class PlataformIndo : IPlataformInfo
    {
        private static readonly bool _isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private static readonly bool _isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public bool IsWindows => _isWindows;
        public bool IsLinux => _isLinux;
    }
}