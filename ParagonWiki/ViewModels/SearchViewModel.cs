using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ParagonWiki.Classes;

namespace ParagonWiki.ViewModels;

    public class SearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand PerformSearch => new Command<string>((string query) =>
        {
            SearchResults = MainPage.singleton.GetSearchResults(query);
        });

        private List<Searchable> searchResults = MainPage.Searchables.ToList();

        private string textSearch = "";
        public List<Searchable> SearchResults
        {
            get
            {
                return searchResults;
            }
            set
            {
                searchResults = value;
                NotifyPropertyChanged();
            }
        }

    public string TextSearch
    {
        get => textSearch;
        set { 
            textSearch = value; 
            NotifyPropertyChanged(); 
        }
    }
    }
