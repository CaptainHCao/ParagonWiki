using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParagonWiki.Classes
{
    public class Item : Searchable 
    {
        // Basics of items
        [JsonProperty(PropertyName = "Stackable")] public bool? Stackable { get; set; }
        [JsonProperty(PropertyName = "MaxQuantityInSlot")] public int? MaxQuantityInSlot { get; set; }
        [JsonProperty(PropertyName = "Effect")] public string? Effect { get; set; }
        [JsonProperty(PropertyName = "Price")] public int? Price { get; set; }
        [JsonProperty(PropertyName = "Quantity")] public int? Quantity { get; set; } // this is the quantity of each item (aka itemSlot quantity)
        [JsonProperty(PropertyName = "EquipmentType")] public string? EquipmentType { get; set; }

        // Basics of quest item
        // list of quest that this item appear to be a quest item
        [JsonProperty(PropertyName = "QuestList")] public List<int>? QuestList { get; set; }
        [JsonProperty(PropertyName = "SavageDescription")] public List<string>? SavageDescription { get; set; }

        // Basics of equipments
        // Armor
        [JsonProperty(PropertyName = "Defense")] public float? Defense { get; set; }
        // Helmet
        [JsonProperty(PropertyName = "SkillDamageAmplifier")] public float? SkillDamageAmplifier { get; set; }
        // Support weapon. Might be more variables to come
        [JsonProperty(PropertyName = "BlockRate")] public float? BlockRate { get; set; }
        [JsonProperty(PropertyName = "Durability")] public float? Durability { get; set; }
        // Primary weapon
        [JsonProperty(PropertyName = "CanBeLeftHanded")] public bool? CanBeLeftHanded { get; set; }
        [JsonProperty(PropertyName = "KnockBackForce")] public float? KnockBackForce { get; set; }
        [JsonProperty(PropertyName = "PhysicDmg")] public float? PhysicDmg { get; set; }
        [JsonProperty(PropertyName = "AbilityDmg")] public float? AbilityDmg { get; set; }
        [JsonProperty(PropertyName = "CriticalChance")] public float? CriticalChance { get; set; }
    }
}
