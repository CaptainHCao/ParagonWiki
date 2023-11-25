using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using ParagonWiki.Classes;

namespace ParagonWiki
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Dialogue> Dialogues { get; set; } = new ObservableCollection<Dialogue>();
        public ObservableCollection<Quest> Quests { get; set; } = new ObservableCollection<Quest>();
        public ObservableCollection<NPC> NPCs { get; set; } = new ObservableCollection<NPC>();
        public static ObservableCollection<Searchable> Searchables { get; set; } = new ObservableCollection<Searchable>();

        FirebaseClient firebaseClient = new FirebaseClient("https://paragon-plus-b130f-default-rtdb.firebaseio.com/");

        public static MainPage singleton;

        public MainPage()
        {
            InitializeComponent();

            //BindingContext = this;

            var ItemCollection = firebaseClient
                                .Child("Items")
                                .AsObservable<Item>()
                                .Subscribe((item) =>
                                {
                                    if (item.Object != null)
                                    {
                                        Items.Add(item.Object);
                                    }
                                });
            var DialogueCollection = firebaseClient
                                .Child("Dialogues")
                                .AsObservable<Dialogue>()
                                .Subscribe((item) =>
                                {
                                    if (item.Object != null)
                                    {
                                        Dialogues.Add(item.Object);
                                    }
                                });
            var NpcCollection = firebaseClient
                                .Child("NPCs")
                                .AsObservable<NPC>()
                                .Subscribe((item) =>
                                {
                                    if (item.Object != null)
                                    {
                                        NPCs.Add(item.Object);
                                    }
                                });
            var QuestCollection = firebaseClient
                                .Child("Quests")
                                .AsObservable<Quest>()
                                .Subscribe((item) =>
                                {
                                    if (item.Object != null)
                                    {
                                        Quests.Add(item.Object);
                                    }
                                });
        }

        private void FetchSearchSource()
        {
            Searchables.Clear();
            foreach (var item in Items) { Searchables.Add(item); }
            foreach (var item in Quests) { Searchables.Add(item); }
            foreach (var item in NPCs) { Searchables.Add(item); }
        }


        private async void ButtonClicked(object sender, EventArgs e)
        {
            //// this code manually pull down data
            //try
            //{
            //    var result = await firebaseClient
            //        .Child("Items").OnceAsListAsync<Item>().
            //    foreach (var item in result)
            //    {
            //        var data = item.Object; // This is an instance of YourDataModel
            //        Items.Add(data);
            //    }

            //} catch (Firebase.Database.FirebaseException ex)
            //{
            //    Debug.WriteLine(ex);
            //}

            Debug.WriteLine(Items.Count);
            Debug.WriteLine(NPCs.Count);
            Debug.WriteLine(Quests.Count);
            Debug.WriteLine(Dialogues.Count);

            FetchSearchSource();
            Debug.WriteLine(Searchables.Count);

        }

        public void OnTextChanged(object sender, EventArgs e)
        {
            FetchSearchSource();
            searchResults.ItemsSource = (from item in Searchables where item.Name.Contains(searchBar.Text, StringComparison.OrdinalIgnoreCase) select item).ToList();
        }

        private async void  lv_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Searchable search = searchResults.SelectedItem as Searchable;
            if (search == null)
            {
                return;
            }
            await Navigation.PushAsync(new DescriptionPage(search), true);
        }
    }
}