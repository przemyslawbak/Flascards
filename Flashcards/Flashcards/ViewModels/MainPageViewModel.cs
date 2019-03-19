using Flashcards.Command;
using Flashcards.DataProvider;
using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

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
        public bool NewPhraseEdit { get; set; }
        public MainPageViewModel(IMainDataProvider dataProvider) //ctor
        {
            Groups = new List<string>();
            _dataProvider = dataProvider;

            //commands
            NewPhraseCommand = new DelegateCommand(OnNewPhraseExecute);
        }

        public ICommand NewPhraseCommand { get; private set; }

        private void OnNewPhraseExecute(object obj)
        {
            throw new NotImplementedException();
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
