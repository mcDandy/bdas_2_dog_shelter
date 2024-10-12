using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BDAS_2_dog_shelter
{
    internal static class Secrets
    {
        internal static readonly string? ConnectionString = JsonDocument.Parse(File.ReadAllText("secrets.json")).RootElement.GetProperty("ConnectionString").GetString();
    }
}
