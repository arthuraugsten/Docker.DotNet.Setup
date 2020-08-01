using System.Globalization;

namespace Docker.DotNet.Setup
{
    internal static class CultureInfoSingleton
    {
        private static readonly CultureInfo _culture = new CultureInfo("en-US");

        public static CultureInfo GetInstance() => _culture;
    }
}