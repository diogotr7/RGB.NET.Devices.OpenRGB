namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBServerDefinition
    {
        public string? ClientName { get; set; }
        public string? Ip { get; set; }
        public int Port { get; set; }
        public bool Connected { get; set; }
        public string? LastError { get; set; }
    }
}