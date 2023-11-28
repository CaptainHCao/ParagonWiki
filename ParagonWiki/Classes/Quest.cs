using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParagonWiki.Classes
{
    public class Quest : Searchable
    {
        // Basics of quests
        [JsonProperty(PropertyName = "QuestType")] public string? QuestType { get; set; }
        [JsonProperty(PropertyName = "QuestGiver")] public string? QuestGiver { get; set; }
        [JsonProperty(PropertyName = "QuestID")] public int? QuestID { get; set; }
        [JsonProperty(PropertyName = "QuestRequirement")] public List<Item>? QuestRequirement { get; set; }
        [JsonProperty(PropertyName = "QuestReward")] public List<Item>? QuestReward { get; set; }


        // attributes that keep track for this app only
        public List<Item> acquiredRewards = new List<Item>();
        public bool acquiredRewardsCalculated = false;
    }
}
