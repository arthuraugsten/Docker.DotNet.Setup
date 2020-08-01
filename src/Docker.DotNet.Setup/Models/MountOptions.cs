namespace Docker.DotNet.Setup.Models
{
    public class MountOptions
    {
        public bool ReadOnly { get; set; } = true;
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
    }
}