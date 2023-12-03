using ParagonWiki.Classes;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace ParagonWiki;

public partial class DescriptionPage : ContentPage
{
	public DescriptionPage(Item item)
	{
		InitializeComponent();
        ItemPage(item);
    }
    public async void ItemPage(Item item)
	{
        itemIcon.Source = item.iconURL != null ? item.iconURL : await SecureStorage.Default.GetAsync("unknowIconURL");

        title.Text = $"{item.Name} ({item.Type})";
        description.Text = item.Description;
        effect.Text = item.Effect != null ? item.Effect : "No special effect has been discovered...";
        maxQuantity.Text = (bool)item.Stackable ? item.MaxQuantityInSlot + " of these items can be carried at the same time in 1 inventory slot."
            : "You can only carry 1 " + item.Name + " in 1 inventory slot. This stuff is heavy.";
        price.Text = "Can be sold for " + item.Price.ToString() + "G";

        // sections for non-basic items
        // 8 cases for None, Weapon, Support Weapon, Armor, Helmet, Cape, Cosmetic, Quest Item
        switch (item.EquipmentType)
        {
            case "None":
                equipmentTitle.Text = "<i>Not an equippable item.</i>";
                break;

            case "Weapon":
                // section for each kind of special item
                equipmentTitle.Text = "<b>Weapon Stats:</b> ";

                if (item.PhysicDmg != null)
                {
                    Label physicDMG = new Label
                    {
                        Text = "Physical damage: " + item.PhysicDmg,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    equipmentStats.Children.Add(physicDMG);
                }
                if (item.AbilityDmg != null)
                {
                    Label abilityDMG = new Label
                    {
                        Text = "Ability damage: " + item.AbilityDmg,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    equipmentStats.Children.Add(abilityDMG);
                }
                if (item.CriticalChance != null)
                {
                    Label crit = new Label
                    {
                        Text = "Critical chance: " + item.CriticalChance,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    equipmentStats.Children.Add(crit);
                }
                if (item.KnockBackForce != null)
                {
                    Label knockback = new Label
                    {
                        Text = "Knockback: " + item.KnockBackForce,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    equipmentStats.Children.Add(knockback);
                }
                if (item.CanBeLeftHanded != null)
                {
                    Label lefthanded = new Label
                    {
                        Text = (bool)item.CanBeLeftHanded ? "Can be used as secondary weapon" 
                        : "Cannot be used as secondary weapon",
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    equipmentStats.Children.Add(lefthanded);
                }
                break;

            case "Quest Item":
                // To-do
                // need to implement slider for savage scale
                break;

            case "Support Weapon":
                equipmentTitle.Text = "<b>Support Weapon Stats:</b> ";

                // iterate through any stats that is not null
                if (item.BlockRate != null) {
                    Label block = new Label
                    {
                        Text = "Block chance: " + item.BlockRate,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    equipmentStats.Children.Add(block);
                }
                if (item.Durability != null) {
                    Label durability = new Label
                    {
                        Text = "Durability: " + item.Durability,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    equipmentStats.Children.Add(durability);
                }
                break;

            case "Armor":
                equipmentTitle.Text = "<b>Armor Stats:</b> ";

                if (item.Defense != null) 
                {
                    Label defense = new Label
                    {
                        Text = "Defense: " + item.Defense,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    equipmentStats.Children.Add(defense);
                }
                break;

            case "Helmet":
                equipmentTitle.Text = "<b>Helmet Stats:</b> ";

                if (item.SkillDamageAmplifier != null)
                {
                    Label skillDamage = new Label
                    {
                        Text = "Skill Damage Amplifier: " + item.SkillDamageAmplifier,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    equipmentStats.Children.Add(skillDamage);
                }
                break;

            case "Cape":
                // currently having no atributes
                // if there are furture attributes, they needs to be added to Item.cs

                equipmentTitle.Text = "<b>Cape Stats:</b> ";
                break;

            case "Cosmetic":
                // currently having no atributes
                // if there are furture attributes, they needs to be added to Item.cs 

                equipmentTitle.Text = "<b>Cosmetic Stats:</b> ";
                break;
        }

        // if there are stats other than the section title
        if (item.EquipmentType != "None" && equipmentStats.Children.Count == 0)
        {
            equipmentTitle.Text = $"<i>No outstanding stats have been recorded for this <b>{item.EquipmentType.ToLower()}</b>.</i>";
        }
    }
}