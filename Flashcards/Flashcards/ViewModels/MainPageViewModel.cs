using Autofac;
using Flashcards.Command;
using Flashcards.DataProvider;
using Flashcards.Models;
using Flashcards.Startup;
using Flashcards.Views;
using LumenWorks.Framework.IO.Csv;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    }
    public class MainPageViewModel : ViewModelBase, IMainPageViewModel
    {
        List<Phrase> oldPhrases = new List<Phrase>(); //verification for PopulateDb method;
        private Func<IPhraseEditViewModel> _phraseEditVmCreator;
        private IMainDataProvider _dataProvider;
        public string FileLocation { get; set; }
        public ObservableCollection<string> Groups { get; set; }
        public List<Phrase> LoadedPhrases { get; set; }
        public bool PhraseEdit { get; set; }
        public IPhraseEditViewModel SelectedPhraseEditViewModel { get; set; }
        public MainPageViewModel(IMainDataProvider dataProvider,
            Func<IPhraseEditViewModel> phraseditVmCreator) //ctor
        {
            _dataProvider = dataProvider;
            _phraseEditVmCreator = phraseditVmCreator;
            Groups = new ObservableCollection<string>();
            LoadedPhrases = new List<Phrase>();
            //commands tests
            AddPhraseCommand = new DelegateCommand(OnNewPhraseExecute);
            LoadFile = new DelegateCommand(OnLoadFileExecute);
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
        private async void OnLoadFileExecute(object obj)
        {
            LoadedPhrases.Clear();
            FileLocation = await _dataProvider.PickUpFile();
            LoadedPhrases = LoadFromFile(FileLocation);
            PopulateDb(LoadedPhrases);
            LoadGroups();
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
            if (filePath != "")
            {
                string stream = "";
                LoadedPhrases.Clear();
                stream = _dataProvider.GetStreamFromCSV(filePath);
                Dictionary<string, int> myPhraseMap = new Dictionary<string, int>(); //exception for wrong format
                var sr = new StringReader(stream);
                using (var csv = new CsvReader(sr, true, '|'))
                {
                    int fieldCount = csv.FieldCount;
                    string[] headers = csv.GetFieldHeaders();
                    for (int i = 0; i < fieldCount; i++)
                    {
                        myPhraseMap[headers[i]] = i;
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
                }
            }
            else
            {
                LoadedPhrases.Clear();
            }
            return LoadedPhrases;
        }
        public void PopulateDb(List<Phrase> phrases)
        {
            if (oldPhrases != phrases) //populates only if collection is new
            {
                foreach (var item in phrases)
                {
                    _dataProvider.SavePhrase(item);
                }
                oldPhrases = phrases;
            }
        }
    }
}
