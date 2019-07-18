using Autofac;
using Flashcards.Command;
using Flashcards.DataProvider;
using Flashcards.Events;
using Flashcards.Models;
using Flashcards.Startup;
using Flashcards.Views;
using LumenWorks.Framework.IO.Csv;
using Plugin.FilePicker;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private IGroupPageViewModel _selectedGroupPageViewModel;
        private IEventAggregator _eventAggregator; //Prism
        List<Phrase> oldPhrases = new List<Phrase>(); //verification for PopulateDb method;
        private Func<IPhraseEditViewModel> _phraseEditVmCreator;
        private Func<IGroupPageViewModel> _groupPageVmCreator;
        private IMainDataProvider _dataProvider;
        public string FileLocation { get; set; }
        public ObservableCollection<string> Groups { get; set; }
        public List<Phrase> LoadedPhrases { get; set; }
        public bool PhraseEdit { get; set; }
        public bool OpenGroup { get; set; }
        public IPhraseEditViewModel SelectedPhraseEditViewModel { get; set; }
        public MainPageViewModel(IMainDataProvider dataProvider,
            Func<IPhraseEditViewModel> phraseditVmCreator,
            Func<IGroupPageViewModel> groupPageVmCreator,
            IEventAggregator eventAggregator) //ctor
        {
            eventAggregator.GetEvent<OpenGroupPageEvent>().Subscribe(OnOpenGroupExecute); //
            _eventAggregator = eventAggregator;
            _dataProvider = dataProvider;
            _phraseEditVmCreator = phraseditVmCreator;
            _groupPageVmCreator = groupPageVmCreator;
            Groups = new ObservableCollection<string>();
            LoadedPhrases = new List<Phrase>();
            //commands tests
            AddPhraseCommand = new DelegateCommand(OnNewPhraseExecute);
            LoadFileCommand = new DelegateCommand(OnLoadFileExecute);
            OpenGroupCommand = new DelegateCommand(OnOpenGroupExecute);

        }

        public ICommand AddPhraseCommand { get; private set; }
        public ICommand LoadFileCommand { get; private set; }
        public ICommand OpenGroupCommand { get; private set; }

        public IGroupPageViewModel SelectedGroupPageViewModel
        {
            get
            {
                return _selectedGroupPageViewModel;
            }
            set
            {
                _selectedGroupPageViewModel = value;
                OnPropertyChanged();
            }
        }
        private void OnOpenGroupExecute(object obj)
        {
            string groupName = (string)obj;
            SelectedGroupPageViewModel = CreateGroupPageViewModel(groupName);
            Application.Current.MainPage.Navigation.PushAsync(new GroupPage(groupName, _eventAggregator));
        }
        private void OnNewPhraseExecute(object obj)
        {
            SelectedPhraseEditViewModel = CreateAndLoadPhraseEditViewModel(null);
        }
        private async void OnLoadFileExecute(object obj)
        {
            LoadedPhrases.Clear();
            FileLocation = await _dataProvider.PickUpFile();
            LoadedPhrases = LoadFromFile(FileLocation);
            PopulateDb(LoadedPhrases);
            LoadGroups();
        }
        private IGroupPageViewModel CreateGroupPageViewModel(string groupName)
        {
            var groupPageVm = _groupPageVmCreator();
            groupPageVm.LoadGroupName(groupName);
            return groupPageVm;
        }
        private IPhraseEditViewModel CreateAndLoadPhraseEditViewModel(int? phraseId)
        {
            var phraseEditVm = _phraseEditVmCreator();
            PhraseEdit = true;
            phraseEditVm.LoadPhrase(phraseId);
            //Application.Current.MainPage.Navigation.PushAsync(new PhraseEditPage());
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
        private bool CsvValidator (string fileContent)
        {
           var fileLines = fileContent.Split(
           new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            if (fileLines.Length < 2)
            {
                return false; //if less than 2 rows, there is no content
            }
            string header = fileLines[0].Replace(" ", String.Empty).ToLower();
            if (header != "name|definition|category|group|priority")
            {
                return false; //if header is wrong
            }
            foreach (var row in fileLines.Skip(1))
            {
                var cells = row.Split('|');
                if (cells.Length != 5)
                {
                    return false; //if in any of rows length of cells is not 4
                }
            }
            return true;
        }
        public List<Phrase> LoadFromFile(string filePath)
        {
            if (filePath != "")
            {
                string stream = "";
                LoadedPhrases.Clear();
                stream = _dataProvider.GetStreamFromCSV(filePath);
                bool validation = CsvValidator(stream);
                if (validation)
                {
                    Dictionary<string, int> myPhraseMap = new Dictionary<string, int>();
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
