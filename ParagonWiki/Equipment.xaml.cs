using CommunityToolkit.Maui.Views;
using Firebase.Database;
using Firebase.Database.Query;
using ParagonWiki.Classes;
using SQLite;
using System.Diagnostics;

namespace ParagonWiki;

public partial class Equipment : ContentPage
{
    SQLiteAsyncConnection conn;
    // slot name according to Item
    public Dictionary<string,Item?> Equipments = new Dictionary<string,Item?>();

    public static Equipment singleton;

    public Equipment()
	{
		InitializeComponent();
        InitializePage();
        singleton = this;
	}
    private async void InitializePage()
    {
        InitializeEquipments();
        await CreateConnection();

        await DisplayEquipments();
    }

    private async void image_Clicked(object sender, EventArgs e)
    {
		await this.ShowPopupAsync(new PopupSearch(ref sender));
    }

    public async Task CreateConnection()
    {
        string libFolder = FileSystem.AppDataDirectory;
        string fname = System.IO.Path.Combine(libFolder, "equipments.db");
        conn = new SQLiteAsyncConnection(fname);
        await conn.CreateTableAsync<Item>();
    }

    public void InitializeEquipments()
    {
        Equipments.Add(helmetSlot.StyleId, null);
        Equipments.Add(armorSlot.StyleId, null);
        Equipments.Add(secondaryWeaponSlot.StyleId, null);
        Equipments.Add(cosmeticSlot.StyleId, null);
        Equipments.Add(capeSlot.StyleId, null);
        Equipments.Add(primaryWeaponSlot.StyleId, null);
    }

    public async Task RetrieveEquipments()
    {
        // in need of testing purpose
        // await conn.DeleteAllAsync<Item>();

        var equipments = await conn.Table<Item>().ToListAsync();
        foreach (var item in equipments)
        {
            Equipments[item.storedSlot] = item;
        }
    }

    public async Task DisplayEquipments()
    {
        await RetrieveEquipments();

        helmetSlot.Source = Equipments[helmetSlot.StyleId] != null ? Equipments[helmetSlot.StyleId].typeIcon : "plus.png";
        armorSlot.Source = Equipments[armorSlot.StyleId] != null ? Equipments[armorSlot.StyleId].typeIcon : "plus.png";
        secondaryWeaponSlot.Source = Equipments[secondaryWeaponSlot.StyleId] != null ? Equipments[secondaryWeaponSlot.StyleId].typeIcon : "plus.png";
        cosmeticSlot.Source = Equipments[cosmeticSlot.StyleId] != null ? Equipments[cosmeticSlot.StyleId].typeIcon : "plus.png";
        capeSlot.Source = Equipments[capeSlot.StyleId] != null ? Equipments[capeSlot.StyleId].typeIcon : "plus.png";
        primaryWeaponSlot.Source = Equipments[primaryWeaponSlot.StyleId] != null ? Equipments[primaryWeaponSlot.StyleId].typeIcon : "plus.png";
    }

    public async Task AddToEquipments(string slotType, Item value)
    {
        try
        {
            // update the history database by removing and adding the same thing. This is just to reorder the database.
            List<Item> aList = await conn.Table<Item>().ToListAsync();
            foreach (var item in aList) 
            {
                Debug.WriteLine(item.storedSlot);
            }

            await conn.Table<Item>().DeleteAsync(dbItem => dbItem.storedSlot != null && dbItem.storedSlot == slotType);
            value.storedSlot = slotType;

            await conn.InsertAsync(value);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            // can't delete the row if the item has not already been there in the table. So just add an entity instead. Not an error.
            value.storedSlot = slotType;
            await conn.InsertAsync(value);
        }
    }
}