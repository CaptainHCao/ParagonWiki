
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
        ImageButton imageButton = sender as ImageButton;
        string equipmentSlot = imageButton.StyleId;
        await this.ShowPopupAsync(new PopupSearch(equipmentSlot));
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

        // update stats everytime we display new item
        UpdateEquipmentStats();
    }

    public async Task AddToEquipments(string slotType, Item value)
    {
        try
        {
            // update the history database by removing and adding the same thing. This is just to reorder the database.
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

    public async Task RemoveFromEquipments(string slotType)
    {
        try
        {
            // remove it from local storage first
            Equipments[slotType] = null;
            // try delete 
            await conn.Table<Item>().DeleteAsync(dbItem => dbItem.storedSlot != null && dbItem.storedSlot == slotType);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    public async void remove_all(object sender, EventArgs e)
    {
        Equipments.Clear();
        InitializeEquipments();
        await conn.DeleteAllAsync<Item>();
        await DisplayEquipments();  
    }

    public async void random_equipments(object sender, EventArgs e)
    {
        //// Delete all first
        //Equipments.Clear();
        //InitializeEquipments();
        //await conn.DeleteAllAsync<Item>();

        PopupSearch helmet = new PopupSearch("helmetSlot");
        PopupSearch armor = new PopupSearch("armorSlot");
        PopupSearch supportWeapon = new PopupSearch("secondaryWeaponSlot");
        PopupSearch cosmetic = new PopupSearch("cosmeticSlot");
        PopupSearch cape = new PopupSearch("capeSlot");
        PopupSearch primaryWeapon = new PopupSearch("primaryWeaponSlot");

        await helmet.random_equip();
        await armor.random_equip();
        await supportWeapon.random_equip();
        await cosmetic.random_equip();
        await primaryWeapon.random_equip();
        await cape.random_equip();

        await DisplayEquipments();
    }

    // stats in their starting points
    public string characterClass;
    public float physicDamage = 0;
    public float magicDamage = 0;
    public float defense = 0;
    public float crit = 0;
    public float evade = 0;
    public float damageAmplifier = 1;
    public float knockback = 0;

    private void ResetStats()
    {
        physicDamage = 0;
        magicDamage = 0;
        defense = 0;
        crit = 0;
        evade = 0;
        damageAmplifier = 1;
        knockback = 0;
    }

    // always call display after update
    // could bind them into 1 delegate but that's overcomplicated. 
    public void UpdateEquipmentStats()
    {
        ResetStats();

        foreach(var element in Equipments)
        {
            Item item = element.Value;
            if (item != null) {
                physicDamage += item.PhysicDmg != null ? (float)item.PhysicDmg : 0;
                magicDamage += item.AbilityDmg != null ? (float)item.AbilityDmg : 0;
                defense += item.Defense != null ? (float)item.Defense : 0;
                crit += item.CriticalChance != null ? (float)item.CriticalChance : 0;
                damageAmplifier *= item.SkillDamageAmplifier != null ? (float)item.SkillDamageAmplifier : 1;
                evade += item.BlockRate != null ? (float)item.BlockRate : 0;
                knockback += item.KnockBackForce != null ? (float)item.KnockBackForce : 0;

                if (item.Effect != null)
                {
                    Label effect = new Label
                    {
                        Text = item.Effect + " (Special effect)",
                        TextType = TextType.Html,
                        FontAttributes = FontAttributes.Italic,
                    };
                    EffectSection.Children.Add(effect);
                }
            }
        }
        DisplayEquipmentStats();
    }

    private void DisplayEquipmentStats()
    {
        ATK.Text = "ATK: " + physicDamage;
        DEF.Text = "DEF: " + defense;
        MAG.Text = "MAG: " + magicDamage;
        CRIT.Text = "CRIT: " + crit;
        EVA.Text = "EVA: " + (100 * evade).ToString("F0") + "%";
        DamageAmplifier.Text = "Damage Amplifier: " + (100 * damageAmplifier).ToString("F0") + "%";
        KnockBack.Text = "Weapon knockback: " + knockback;

        if (EffectSection.Children.Count == 0) {
            EffectSection.Children.Clear();
            Label notify = new Label
            {
                Text = "This combination withholds any special effects.",
                TextType = TextType.Html,
                FontAttributes = FontAttributes.Italic,
            };
            EffectSection.Children.Add(notify);
        } 
    }
}