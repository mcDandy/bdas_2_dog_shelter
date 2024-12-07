using System.IO;
using System.Text.Json;

namespace BDAS_2_dog_shelter
{
    internal static class Secrets
    {
        internal static readonly string? ConnectionString = JsonDocument.Parse(File.ReadAllText("secrets.json")).RootElement.GetProperty("ConnectionString").GetString();
    }
}
