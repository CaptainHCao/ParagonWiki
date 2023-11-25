using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParagonWiki.Classes
{
    public class Dialogue 
    { 
        // Basics of dialogues
        [JsonProperty(PropertyName = "Owner")] public string? Owner { get; set; }
        [JsonProperty(PropertyName = "QuestID")] public int? QuestID { get; set; }
        [JsonProperty(PropertyName = "State")] public string? State { get; set; }
        [JsonProperty(PropertyName = "Sentences")] public string[]? Sentences { get; set; }
    }
}
