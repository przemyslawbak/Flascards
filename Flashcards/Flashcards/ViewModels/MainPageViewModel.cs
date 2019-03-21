using Flashcards.Command;
using Flashcards.DataProvider;
using Flashcards.Models;
using Flashcards.Views;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Flashcards.ViewModels
{
    public interface IMainPageViewModel
    {
        void LoadGroups();
        List<Phrase> LoadFromFile(string filePath);
    }
    public class MainPageViewModel : ViewModelBase, IMainPageViewModel
    {
        private IPhraseEditViewModel _selectedPhraseEditViewModel;
        private Func<IPhraseEditViewModel> _phraseEditVmCreator;
        private IMainDataProvider _dataProvider;
        public List<string> Groups { get; set; }
        public List<Phrase> LoadedPhrases { get; set; }
        public bool PhraseEdit { get; set; }
        public MainPageViewModel(IMainDataProvider dataProvider,
            Func<IPhraseEditViewModel> phraseditVmCreator) //ctor
        {
            _dataProvider = dataProvider;
            _phraseEditVmCreator = phraseditVmCreator;
            Groups = new List<string>();
            LoadedPhrases = new List<Phrase>();
            //commands
            AddPhraseCommand = new DelegateCommand(OnNewPhraseExecute);
        }

        public ICommand AddPhraseCommand { get; private set; }
        public ICommand LoadFile { get; private set; }

        private void OnNewPhraseExecute(object obj)
        {
            SelectedPhraseEditViewModel = CreateAndLoadPhraseEditViewModel(null);
        }

        private IPhraseEditViewModel CreateAndLoadPhraseEditViewModel(int? phraseId)
        {
            //Application.Current.MainPage.Navigation.PushAsync(new PhraseEditPage());
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
        public List<Phrase> LoadFromFile(string filePath)
        {
            string stream = "";
            LoadedPhrases.Clear();
            if (File.Exists(filePath))
            {
                stream = _dataProvider.GetStreamFromCSV(filePath);
            }
            else
            {
                stream = @"Name | Definition | Category | Group | Priority
error1 | def1 | cat1 | gr1 | prio1
error2 | def2 | cat2 | gr2 | prio2";
            }
            Dictionary<string, int> myPhraseMap = new Dictionary<string, int>();
            var sr = new StringReader(stream);
            using (var csv = new CsvReader(sr, true, '|'))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                for (int i = 0; i < fieldCount; i++)
                {
                    myPhraseMap[headers[i]] = i; // track the index of each column name
                }
                while (csv.ReadNextRecord())
                {
                    Phrase phrase = new Phrase
                    {
                        Name = csv[myPhraseMap["Name"]],
                        Definition = csv[myPhraseMap["Definition"]],
                        Category = csv[myPhraseMap["Category"]],
                        Group = csv[myPhraseMap["Group"]],
                        Priority = csv[myPhraseMap["Priority"]],
                        Learned = false
                    };
                    LoadedPhrases.Add(phrase);
                }
                return LoadedPhrases;
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
