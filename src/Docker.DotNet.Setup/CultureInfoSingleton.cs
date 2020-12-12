using System.Globalization;

namespace Docker.DotNet.Setup
{
    internal static class CultureInfoSingleton
    {
        private const string CultureName = "en-US";

        private static readonly CultureInfo _culture = new CultureInfo(CultureName);

        public static CultureInfo GetInstance() => _culture;
    }
}