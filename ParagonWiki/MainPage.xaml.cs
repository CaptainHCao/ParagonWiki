using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.CompilerServices;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using ParagonWiki.Classes;
using SQLite;

namespace ParagonWiki
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Dialogue> Dialogues { get; set; } = new ObservableCollection<Dialogue>();
        public ObservableCollection<Quest> Quests { get; set; } = new ObservableCollection<Quest>();
        public ObservableCollection<NPC> NPCs { get; set; } = new ObservableCollection<NPC>();
        public static ObservableCollection<Searchable> Searchables { get; set; } = new ObservableCollection<Searchable>();

        public static List<Searchable> history { get; set; } = new List<Searchable>();

        FirebaseClient firebaseClient = new FirebaseClient("https://paragon-plus-b130f-default-rtdb.firebaseio.com/");

        public static MainPage singleton;

        SQLiteAsyncConnection conn;

        public async void InitializeConstants()
        {
            await SecureStorage.Default.SetAsync("unknownItemIconURL",
                "https://firebasestorage.googleapis.com/v0/b/paragon-plus-b130f.appspot.com/o/icons%2Finventory.png?alt=media&token=57667942-0ca3-4443-84b2-5d5a3a064f9d");
            await SecureStorage.Default.SetAsync("unknownQuestIconURL",
                "https://firebasestorage.googleapis.com/v0/b/paragon-plus-b130f.appspot.com/o/icons%2Fmagic.png?alt=media&token=aeb464a3-0732-4e6e-bcfc-e14285e3eb8f");
            await SecureStorage.Default.SetAsync("unknowNpcIconURL",
                "https://firebasestorage.googleapis.com/v0/b/paragon-plus-b130f.appspot.com/o/icons%2Fcharacter.png?alt=media&token=09193acd-1dfc-4b45-88f6-da6598d1af35");
            await SecureStorage.Default.SetAsync("unknowIconURL",
                "https://firebasestorage.googleapis.com/v0/b/paragon-plus-b130f.appspot.com/o/icons%2Fquestion.png?alt=media&token=b5c05f86-8d9e-477f-8d06-c856a1ff8e59");
        }

        public async Task CreateConnection()
        {
            string libFolder = FileSystem.AppDataDirectory;
            string fname = System.IO.Path.Combine(libFolder, "history.db");
            conn = new SQLiteAsyncConnection(fname);
            await conn.CreateTableAsync<Searchable>();
        }

        public MainPage()
        {
            InitializeComponent();

            singleton = this;

            //BindingContext = this;

            //var ItemCollection = firebaseClient
            //                    .Child("Items")
            //                    .AsObservable<Item>()
            //                    .Subscribe((item) =>
            //                    {
            //                        if (item.Object != null)
            //                        {
            //                            Items.Add(item.Object);
            //                        }
            //                    });
            //var DialogueCollection = firebaseClient
            //                    .Child("Dialogues")
            //                    .AsObservable<Dialogue>()
            //                    .Subscribe((item) =>
            //                    {
            //                        if (item.Object != null)
            //                        {
            //                            Dialogues.Add(item.Object);
            //                        }
            //                    });
            //var NpcCollection = firebaseClient
            //                    .Child("NPCs")
            //                    .AsObservable<NPC>()
            //                    .Subscribe((item) =>
            //                    {
            //                        if (item.Object != null)
            //                        {
            //                            NPCs.Add(item.Object);
            //                        }
            //                    });
            //var QuestCollection = firebaseClient
            //                    .Child("Quests")
            //                    .AsObservable<Quest>()
            //                    .Subscribe((item) =>
            //                    {
            //                        if (item.Object != null)
            //                        {
            //                            Quests.Add(item.Object);
            //                        }
            //                    });

            InitData();
            InitializeConstants();
        }

        protected async void InitData()
        {
            // Get the data from the database
            var fireBaseItems = (await firebaseClient
                                .Child("Items")
                                .OnceAsListAsync
                                <Item>());
            foreach (var item in fireBaseItems)
            {
                Items.Add(item.Object);
            }
            var fireBaseDialogues = (await firebaseClient
                                .Child("Dialogues")
                                .OnceAsListAsync
                                <Dialogue>());
            foreach (var item in fireBaseDialogues)
            {
                Dialogues.Add(item.Object);
            }
            var fireBaseNPCs = (await firebaseClient
                                .Child("NPCs")
                                .OnceAsListAsync
                                <NPC>());
            foreach (var item in fireBaseNPCs)
            {
                NPCs.Add(item.Object);
            }
            var fireBaseQuests = (await firebaseClient
                                .Child("Quests")
                                .OnceAsListAsync
                                <Quest>());
            foreach (var item in fireBaseQuests)
            {
                Quests.Add(item.Object);
            }

            // fectch the searchable item list
            await FetchSearchSource();
            // update the history
            await CreateConnection();
            await matchHistoryItems();
        }

        private async Task FetchSearchSource()
        {
            Searchables.Clear();

            // items filled
            foreach (var item in Items) {
                if (item.iconURL == null)
                {
                    item.typeIcon = await SecureStorage.Default.GetAsync("unknownItemIconURL");
                } else
                {
                    item.typeIcon = item.iconURL;
                }
                Searchables.Add(item);
            }

            // quests filled
            foreach (var item in Quests) {
                Searchables.Add(item);
                item.typeIcon = await SecureStorage.Default.GetAsync("unknownQuestIconURL");
            }

            // npcs filled
            FetchNPCs();
            foreach (var item in NPCs) {
                Searchables.Add(item);
                if (item.iconURL == null)
                {
                    item.typeIcon = await SecureStorage.Default.GetAsync("unknowNpcIconURL");
                }
            }
        }

        private void FetchNPCs()
        {
            foreach (NPC npc in NPCs) {
                npc.dialogues = (from item in Dialogues where item.Owner.Equals(npc.Name, StringComparison.OrdinalIgnoreCase) orderby item.QuestID select item).ToList();
            }
        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            // testing code
            Debug.WriteLine(Items.Count);
            Debug.WriteLine(NPCs.Count);
            Debug.WriteLine(Quests.Count);
            Debug.WriteLine(Dialogues.Count);

            Debug.WriteLine(Searchables.Count);

        }

        protected async override void OnAppearing()
        {
            // fix the bug where it does not re-locate the latest searched item to the top of the history after you hit the back button.
            if (searchBar.Text == "")
            {
                await matchHistoryItems();
            }

            if (searchResults.ItemsSource != null)
            {
                // restart the scroll to the top everytime back to this main page.
                searchResults.ScrollTo((searchResults.ItemsSource as List<Searchable>).First(), 0, false);
            }
        }

        public async void OnTextChanged(object sender, EventArgs e)
        {
            if (searchBar.Text == "")
            {
                await matchHistoryItems();
            }
            else
            {
                List<Searchable> prioResults = (from item in Searchables where item.Name.StartsWith(searchBar.Text, StringComparison.OrdinalIgnoreCase) select item).ToList();

                List<Searchable> secondResults = (from item in Searchables where item.Name.Contains(searchBar.Text, StringComparison.OrdinalIgnoreCase) select item).ToList();

                searchResults.ItemsSource = prioResults.Union(secondResults).ToList();
            }
        }

        private async Task matchHistoryItems()
        {
            // in need of testing purpose
            //await conn.DeleteAllAsync<Searchable>();

            if (conn == null) return;
            history = await conn.Table<Searchable>().OrderBy(item => item.Id).ToListAsync();

            for (int i = 0; i < history.Count; i++)
            {
                history[i] = (from item in Searchables where item.Name == history[i].Name && item.Type == history[i].Type select item).FirstOrDefault();
            }

            // Showing latest searches then older searches
            history.Reverse();
            searchResults.ItemsSource = history;
        }

        private async void searchResults_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Searchable search = searchResults.SelectedItem as Searchable;
            if (search == null)
            {
                return;
            }

            try
            {
                // update the history database by removing and adding the same thing. This is just to reorder the database.
                await conn.Table<Searchable>().DeleteAsync(dbItem => dbItem.Name == search.Name && dbItem.Type == search.Type);
                await conn.InsertAsync(new Searchable { Name = search.Name, Description = search.Description, Type = search.Type });
            } catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                // can't delete the row if the item has not already been there in the table. So just add an entity instead. Not an error.
                await conn.InsertAsync(new Searchable { Name = search.Name, Description = search.Description, Type = search.Type });
            }

            if (search is Item item) {
                await Navigation.PushAsync(new DescriptionPage(item), true);
            }
            if (search is NPC npc)
            {
                await Navigation.PushAsync(new NpcPage(npc), true);
            }
            if (search is Quest quest)
            {
                AccumulateRewards(quest);
                await Navigation.PushAsync(new QuestPage(quest), true);
            }
        }

        private void AccumulateRewards(Quest quest)
        {
            // singleton for this method to avoid running redundant loops many times
            if (!quest.acquiredRewardsCalculated)
            {
                for (int i = 1; i < quest.QuestID; i++)
                {
                    Quest aPreviousQuest = (from item in Quests where item.QuestID == i select item).First();

                    // Only process if this quest have rewards.
                    if (aPreviousQuest.QuestReward != null)
                    {
                        foreach (var reward in aPreviousQuest.QuestReward)
                        {
                            bool found = false;
                            for (int j = 0; j < quest.acquiredRewards.Count; j++)
                            {
                                // if the item has already been in the pool, simply update the quantity
                                if (reward.Name.Equals(quest.acquiredRewards[j].Name, StringComparison.OrdinalIgnoreCase))
                                {
                                    found = true;
                                    quest.acquiredRewards[j].Quantity += reward.Quantity;
                                    break;
                                }
                            }
                            // cant find the item with that name in the pool, add a new on
                            if (!found)
                            {
                                quest.acquiredRewards.Add(reward);
                            }
                        }
                    }

                }
                quest.acquiredRewardsCalculated = true;
            }
        }
    }
}