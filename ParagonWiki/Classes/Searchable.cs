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
        [JsonProperty(PropertyName = "Description")] public string? Description { get; set; }
        [JsonProperty(PropertyName = "Name")] [Column("_name"), Unique] public string Name { get; set; }
    }
}
