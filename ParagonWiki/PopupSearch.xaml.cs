using CommunityToolkit.Maui.Views;
using ParagonWiki.Classes;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ParagonWiki;

public partial class PopupSearch : Popup
{
    private List<Item> ItemPool = null;
    public string typeStoredInDb;
    public string equipmentType;
    public ImageButton imageButton;

    public PopupSearch(ref object caller)
	{
		InitializeComponent();

        imageButton = caller as ImageButton;
        string equipmentSlot = imageButton.StyleId;

        // secondary weapon works differently. 
        if (equipmentSlot == "secondaryWeaponSlot")
        {
            ItemPool = (from item in MainPage.singleton.Items where
                        item.EquipmentType == "Support Weapon" || 
                        (item.EquipmentType == "Weapon" && (bool)item.CanBeLeftHanded) select item).ToList();
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
            ItemPool = (from item in MainPage.singleton.Items where item.EquipmentType == equipmentType select item).ToList();
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

        // add item to database
        await Equipment.singleton.AddToEquipments(imageButton.StyleId, itemChosen);

        // update the display
        await Equipment.singleton.DisplayEquipments();

        Close();
    }
}