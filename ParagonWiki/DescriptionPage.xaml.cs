using ParagonWiki.Classes;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace ParagonWiki;

public partial class DescriptionPage : ContentPage
{
	public DescriptionPage(Searchable searchItem)
	{
		InitializeComponent();
		InitialPage(searchItem);
	}

	public void InitialPage(Searchable searchItem)
	{
		if (searchItem == null) { return; }
		if (searchItem is Item item)
		{
            ItemPage(item);
        }
    }
	public void ItemPage(Item item)
	{
		VerticalStackLayout container = new VerticalStackLayout
		{
			HorizontalOptions = LayoutOptions.Center,
			Padding = 50
		};
		Label title = new Label
		{
			Text = item.Name,
			TextType = TextType.Html,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Bold,
			FontSize = 30
		};
        Label description = new Label
        {
            Text = item.Description,
            TextType = TextType.Html,
            HorizontalOptions = LayoutOptions.Center,
            FontAttributes = FontAttributes.Italic,
			FontSize = 20
        };

        Label itemSection = new Label
        {
            Text = "<b>Item Stats:</b> ",
            TextType = TextType.Html,
            FontSize = 25
        };

        string itemEffect = string.Empty;
		if (item.Effect != null)
		{
			itemEffect = item.Effect;
		} else
		{
			itemEffect = "No special effect has been discovered...";
		}

        Label effect = new Label
        {
            Text = itemEffect,
            TextType = TextType.Html,
            FontSize = 20
        };

        int? maxQuantityInSlot = 1;
        if ((bool)item.Stackable)
        {
            maxQuantityInSlot = item.MaxQuantityInSlot;
        }

        Label maxQuantity = new Label
        {
            Text = maxQuantityInSlot == 1 ? "You can only carry 1 " + item.Name + " in 1 inventory slot. This stuff is heavy." 
            : maxQuantityInSlot + " of these items can be carried at the same time in 1 inventory slot.",
            TextType = TextType.Html,
            FontSize = 20
        };

        Label price = new Label
        {
            Text = "Can be sold for " + item.Price.ToString() + "G",
            TextType = TextType.Html,
            FontSize = 20
        };

        Content = container;
        container.Children.Add(title);
        container.Children.Add(description);
        container.Children.Add(itemSection);
        container.Children.Add(effect);
        container.Children.Add(maxQuantity);
        container.Children.Add(price);

        // wrap around the stats 
        VerticalStackLayout statsContainer = new VerticalStackLayout();

        // sections for non-basic items
        // 8 cases for None, Weapon, Support Weapon, Armor, Helmet, Cape, Cosmetic, Quest Item
        switch (item.EquipmentType)
        {
            case "None":
                break;

            case "Weapon":
                // section for each kind of special item
                Label weaponSection = new Label
                {
                    Text = "<b>Weapon Stats:</b> ",
                    TextType = TextType.Html,
                    FontSize = 25
                };
                statsContainer.Children.Add(weaponSection);

                if (item.PhysicDmg != null)
                {
                    Label physicDMG = new Label
                    {
                        Text = "Physical damage: " + item.PhysicDmg,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    statsContainer.Children.Add(physicDMG);
                }
                if (item.AbilityDmg != null)
                {
                    Label abilityDMG = new Label
                    {
                        Text = "Ability damage: " + item.AbilityDmg,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    statsContainer.Children.Add(abilityDMG);
                }
                if (item.CriticalChance != null)
                {
                    Label crit = new Label
                    {
                        Text = "Critical chance: " + item.CriticalChance,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    statsContainer.Children.Add(crit);
                }
                if (item.KnockBackForce != null)
                {
                    Label knockback = new Label
                    {
                        Text = "Knockback: " + item.KnockBackForce,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    statsContainer.Children.Add(knockback);
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
                    statsContainer.Children.Add(lefthanded);
                }
                break;

            case "Quest Item":
                // To-do
                // need to implement slider for savage scale
                break;

            case "Support Weapon":
                Label supWeaponSection = new Label
                {
                    Text = "<b>Support Weapon Stats:</b> ",
                    TextType = TextType.Html,
                    FontSize = 25
                };
                statsContainer.Children.Add(supWeaponSection);

                // iterate through any stats that is not null
                if (item.BlockRate != null) {
                    Label block = new Label
                    {
                        Text = "Block chance: " + item.BlockRate,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    statsContainer.Children.Add(block);
                }
                if (item.Durability != null) {
                    Label durability = new Label
                    {
                        Text = "Durability: " + item.Durability,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    statsContainer.Children.Add(durability);
                }
                break;

            case "Armor":
                Label armorSection = new Label
                {
                    Text = "<b>Armor Stats:</b> ",
                    TextType = TextType.Html,
                    FontSize = 25
                };
                statsContainer.Children.Add(armorSection);

                if (item.Defense != null) 
                {
                    Label defense = new Label
                    {
                        Text = "Defense: " + item.Defense,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    statsContainer.Children.Add(defense);
                }
                break;

            case "Helmet":
                Label HelmetSection = new Label
                {
                    Text = "<b>Helmet Stats:</b> ",
                    TextType = TextType.Html,
                    FontSize = 25
                };
                statsContainer.Children.Add(HelmetSection);

                if (item.SkillDamageAmplifier != null)
                {
                    Label skillDamage = new Label
                    {
                        Text = "Skill Damage Amplifier: " + item.SkillDamageAmplifier,
                        TextType = TextType.Html,
                        FontSize = 20
                    };
                    statsContainer.Children.Add(skillDamage);
                }
                break;

            case "Cape":
                Label CapeSection = new Label
                {
                    Text = "<b>Cape Stats:</b> ",
                    TextType = TextType.Html,
                    FontSize = 25
                };
                statsContainer.Children.Add(CapeSection);

                // currently having no atributes
                // if there are furture attributes, they needs to be added to Item.cs                
                break;

            case "Cosmetic":
                Label CosmeticSection = new Label
                {
                    Text = "<b>Cosmetic Stats:</b> ",
                    TextType = TextType.Html,
                    FontSize = 25
                };
                statsContainer.Children.Add(CosmeticSection);

                // currently having no atributes
                // if there are furture attributes, they needs to be added to Item.cs 
                break;
        }

        // if there are stats other than the section title
        if (statsContainer.Children.Count > 1)
        {
            container.Children.Add(statsContainer);
        }
        else if (statsContainer.Children.Count == 1)
        {
            container.Children.Add(new Label
            {
                Text = $"<i>No outstanding stats have been recorded for this <b>{item.EquipmentType.ToLower()}</b>.</i>",
                TextType = TextType.Html,
                FontSize = 20
            });
        }
    }
}