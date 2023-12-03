using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ParagonWiki.Classes
{
    public class Searchable
    {
        // id for the table in local database
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        // Basics of all objects (including effects)
        [JsonProperty(PropertyName = "Type")] public string Type { get; set; }

        [JsonProperty(PropertyName = "Name")][Column("_name"), Unique] public string Name { get; set; }
        
        // Ignore in SQLite
        [Ignore][JsonProperty(PropertyName = "IconURL")] public string iconURL { get; set; }
        [Ignore][JsonProperty(PropertyName = "Description")] public string? Description { get; set; }
        [Ignore] public string typeIcon { get; set; }
        [Ignore] public string Icon { get; set; }
    }
}
