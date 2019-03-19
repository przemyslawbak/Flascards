using Flashcards.Command;
using Flashcards.DataProvider;
using Flashcards.Models;
using Flashcards.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Flashcards.ViewModels
{
    public interface IMainPageViewModel
    {
        void LoadGroups();
    }
    public class MainPageViewModel : ViewModelBase, IMainPageViewModel
    {
        private IPhraseEditViewModel _selectedPhraseEditViewModel;
        private Func<IPhraseEditViewModel> _phraseEditVmCreator;
        private IMainDataProvider _dataProvider;
        public List<string> Groups { get; set; }
        public bool PhraseEdit { get; set; }
        public MainPageViewModel(IMainDataProvider dataProvider,
            Func<IPhraseEditViewModel> phraseditVmCreator) //ctor
        {
            _phraseEditVmCreator = phraseditVmCreator;
            Groups = new List<string>();
            _dataProvider = dataProvider;
            //commands
            AddPhraseCommand = new DelegateCommand(OnNewPhraseExecute);
        }

        public ICommand AddPhraseCommand { get; private set; }

        private void OnNewPhraseExecute(object obj)
        {
            SelectedPhraseEditViewModel = CreateAndLoadPhraseEditViewModel(null);
        }

        private IPhraseEditViewModel CreateAndLoadPhraseEditViewModel(int? phraseId)
        {
            var phraseEditVm = _phraseEditVmCreator();
            PhraseEdit = true;
            phraseEditVm.LoadPhrase(phraseId);
            return phraseEditVm;
        }

        public void LoadGroups() //loads group list from the DB
        {
            Groups.Clear();
            foreach (var group in _dataProvider.GetGroups())
            {
                Groups.Add(group);
            }
        }
        public IPhraseEditViewModel SelectedPhraseEditViewModel
        {
            get
            {
                return _selectedPhraseEditViewModel;
            }

            set
            {
                _selectedPhraseEditViewModel = value;
                OnPropertyChanged();
            }
        }
    }
}
