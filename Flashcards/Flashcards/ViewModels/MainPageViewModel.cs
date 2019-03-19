using Flashcards.Command;
using Flashcards.DataProvider;
using Flashcards.Models;
using Flashcards.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Flashcards.ViewModels
{
    public interface IMainPageViewModel
    {
        void LoadGroups();
    }
    public class MainPageViewModel : IMainPageViewModel
    {
        private IMainDataProvider _dataProvider;
        public List<string> Groups { get; set; }
        public bool PhraseEdit { get; set; }
        public MainPageViewModel(IMainDataProvider dataProvider) //ctor
        {
            Groups = new List<string>();
            _dataProvider = dataProvider;

            //commands
            AddPhraseCommand = new DelegateCommand(OnNewPhraseExecute);
        }

        public ICommand AddPhraseCommand { get; private set; }

        async public void OnNewPhraseExecute(object obj) //open new phrase page
        {
            PhraseEdit = true;
            await Application.Current.MainPage.Navigation.PushAsync(new PhraseEditPage());
        }

        public void LoadGroups() //loads group list from the DB
        {
            Groups.Clear();
            foreach (var group in _dataProvider.GetGroups())
            {
                Groups.Add(group);
            }
        }
    }
}
