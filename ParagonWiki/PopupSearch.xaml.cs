using CommunityToolkit.Maui.Views;
using ParagonWiki.Classes;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ParagonWiki;

public partial class PopupSearch : Popup
{
    private List<Item> ItemPool = new List<Item>();
    private List<Item> validItemPool = new List<Item>();    
    public string typeStoredInDb;
    public string equipmentType;
    //public ImageButton imageButton;
    string equipmentSlot;

    public PopupSearch(string slot)
	{
		InitializeComponent();

        //imageButton = caller as ImageButton;
        //string equipmentSlot = imageButton.StyleId;

        equipmentSlot = slot;

        ItemPool.Add(new Item { Name = "None" }); 
        
        // secondary weapon works differently. 
        if (equipmentSlot == "secondaryWeaponSlot")
        {
            validItemPool = (from item in MainPage.singleton.Items
                             where item.EquipmentType == "Support Weapon" ||
                             (item.EquipmentType == "Weapon" && (bool)item.CanBeLeftHanded)
                             select item).ToList();
            ItemPool.AddRange(validItemPool);
        } else
        {
            switch (equipmentSlot)
            {
                case "helmetSlot":
                    equipmentType = "Helmet";
                    break;
                case "armorSlot":
                    equipmentType = "Armor";
                    break;
                case "cosmeticSlot":
                    equipmentType = "Cosmetic";
                    break;
                case "primaryWeaponSlot":
                    equipmentType = "Weapon";
                    break;
                case "capeSlot":
                    equipmentType = "Cape";
                    break;
            }
            validItemPool = (from item in MainPage.singleton.Items where item.EquipmentType == equipmentType select item).ToList();
            ItemPool.AddRange(validItemPool);
        }

        OnTextChanged(null, null);
    }

    public void OnTextChanged(object sender, EventArgs e)
    {
        List<Item> prioResults = (from item in ItemPool where item.Name.StartsWith(searchBar.Text, StringComparison.OrdinalIgnoreCase) select item).ToList();

        List<Item> secondResults = (from item in ItemPool where item.Name.Contains(searchBar.Text, StringComparison.OrdinalIgnoreCase) select item).ToList();

        searchResults.ItemsSource = prioResults.Union(secondResults).ToList();
    }

    private async void searchResults_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Item itemChosen = searchResults.SelectedItem as Item;
        if (itemChosen == null)
        {
            return;
        }
        else if (itemChosen.Name == "None")
        {
            await Equipment.singleton.RemoveFromEquipments(equipmentSlot);
        }
        else
        {
            // add item to database
            await Equipment.singleton.AddToEquipments(equipmentSlot, itemChosen);
        }
        // update the display
        await Equipment.singleton.DisplayEquipments();

        Close();
    }

    public async Task random_equip()
    {
        // add item to database
        await Equipment.singleton.AddToEquipments(equipmentSlot, randomItem());
    }

    private Item randomItem()
    {
        // Create a Random object
        Random random = new Random();

        // Get a random index excluding the first element
        int randomIndex = random.Next(0, validItemPool.Count);

        // Retrieve the random element
        return validItemPool[randomIndex];
    }
}