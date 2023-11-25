using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParagonWiki.Classes
{
    public class Searchable
    {
        // Basics of all objects (including effects)
        [JsonProperty(PropertyName = "Type")] public string Type { get; set; }
        [JsonProperty(PropertyName = "Description")] public string? Description { get; set; }
        [JsonProperty(PropertyName = "Name")] public string Name { get; set; }
    }
}
