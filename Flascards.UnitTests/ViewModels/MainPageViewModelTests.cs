using Flashcards.DataProvider;
using Flashcards.Models;
using Flashcards.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flascards.UnitTests.ViewModels
{
    public class MainPageViewModelTests
    {
        List<Phrase> phrases;
        private MainPageViewModel _viewModel;
        private Mock<IPhraseEditViewModel> _phraseEditViewModelMock;
        private Mock<IMainDataProvider> _mainDataProviderMock;
        public MainPageViewModelTests()
        {
            //instances
            phrases = new List<Phrase>
            {
                new Phrase { Category = "newCat1", Definition = "newDef1", Group = "newGr1", Learned = false, Name = "newName1", Priority = "newPrio1", Id = 7 }
            };
            _phraseEditViewModelMock = new Mock<IPhraseEditViewModel>();
            _mainDataProviderMock = new Mock<IMainDataProvider>();

            //setup
            _mainDataProviderMock.Setup(dp => dp.GetGroups())
                .Returns(new List<string>
                {
          "Group #1",
          "Group #2",
          "Group #3"
             });
            _mainDataProviderMock.Setup(dp => dp.PickUpFile())
                .ReturnsAsync("goodData.csv");
            _mainDataProviderMock.Setup(dp => dp.GetStreamFromCSV("goodData.csv"))
                 .Returns("Name| Definition|Category|Group|Priority\nname1 |def1|cat1|gr1|prio1\nname2 |def2|cat2|gr2|prio2");
            _mainDataProviderMock.Setup(dp => dp.GetStreamFromCSV("emptyData.csv"))
                .Returns("");
            _mainDataProviderMock.Setup(dp => dp.GetStreamFromCSV("notValidData.csv"))
                .Returns("% PDF - 1.5\n% âăĎÓ\n735 0 obj\n<</ Linearized 1 / L 1120326 / O 737 / E 573552 / N 4 / T 1119849 / H[491 262] >>\nendobj");
            //VM instance
            _viewModel = new MainPageViewModel(_mainDataProviderMock.Object, CreatePhraseEditViewModel);
        }

        private IPhraseEditViewModel CreatePhraseEditViewModel() //method for creating PhraseEditVM
        {
            var phraseEditViewModelMock = new Mock<IPhraseEditViewModel>();
            phraseEditViewModelMock.Setup(vm => vm.LoadPhrase(It.IsAny<int>()))
              .Callback<int?>(phraseId =>
              {
                  phraseEditViewModelMock.Setup(vm => vm.Phrase)
            .Returns(new Phrase());
              });
            _phraseEditViewModelMock = phraseEditViewModelMock; //field = var(!!)
            return phraseEditViewModelMock.Object;
        }

        [Fact]
        public void LoadGroups_ShouldLoadOnce_True()
        {
            _viewModel.LoadGroups(); //loads groups twice
            _viewModel.LoadGroups();

            Assert.Equal(3, _viewModel.Groups.Count); //counts how many groups are loaded
        }
        [Fact]
        public void LoadGroups_ShouldLoad_True()
        {
            _viewModel.LoadGroups(); //loads collection of groups (from setup)
            Assert.Equal(3, _viewModel.Groups.Count); //counts groups
            var phrase = _viewModel.Groups[0];
            Assert.NotNull(phrase);
            Assert.Equal("Group #1", phrase); //compares group name
        }
        [Fact]
        public void AddPhrase_ShouldBeExecuted_True()
        {
            _viewModel.PhraseEdit = false; //set up PhraseEdit prop
            _viewModel.AddPhraseCommand.Execute(null); // executes command
            Assert.True(_viewModel.PhraseEdit); //verifies PhraseEdit prop
            _phraseEditViewModelMock.Verify(vm => vm.LoadPhrase(null), Times.Once); //counts loaded phrases
        }
        [Fact]
        public void LoadFromFile_ShouldConvertReturnedCorrectFormatString_ReturnsPhraseList()
        {
            _viewModel.LoadFromFile("goodData.csv"); //loads phrases from the file
            Assert.Equal(2, _viewModel.LoadedPhrases.Count); //counts loaded phrases from the file
            var phrase = _viewModel.LoadedPhrases[0];
            Assert.NotNull(phrase); //checks if phrase is not null, below compares props
            Assert.Equal("name1", phrase.Name);
            Assert.Equal("def1", phrase.Definition);
            Assert.Equal("cat1", phrase.Category);
            Assert.Equal("gr1", phrase.Group);
            Assert.Equal("prio1", phrase.Priority);
        }
        [Fact]
        public void PopulateDb_ShouldSeedDbWithPhrases_CallsDpSavePhrase()
        {
            _viewModel.LoadedPhrases = phrases; //populates collection
            _viewModel.PopulateDb(_viewModel.LoadedPhrases); //populates Db with phase list - 1 item
            _mainDataProviderMock.Verify(dp => dp.SavePhrase(It.IsAny<Phrase>()), Times.Once); //counts saved phrases
        }
        [Fact]
        public void LoadFile_ShouldBeExecuted_CallsOnLoadFileExecute()
        {
            _viewModel.LoadFileCommand.Execute(null); //execute command
            Assert.Equal(2, _viewModel.LoadedPhrases.Count()); //counts loaded phrases from the file
            Assert.Equal(3, _viewModel.Groups.Count); //counts loaded groups
            _mainDataProviderMock.Verify(dp => dp.SavePhrase(It.IsAny<Phrase>()), Times.AtLeast(2)); //counts saved phrases
        }
        [Fact]
        public void PopulateDb_ShouldSeedDbOnce_True()
        {
            _viewModel.LoadedPhrases = phrases; //populates collection
            _viewModel.PopulateDb(_viewModel.LoadedPhrases); //seeds Db twice
            _viewModel.PopulateDb(_viewModel.LoadedPhrases);
            _mainDataProviderMock.Verify(dp => dp.SavePhrase(It.IsAny<Phrase>()), Times.Once); //should seed only once
        }
        [Fact]
        public void LoadFromFile_WithFilePathParameterIsNull_ReturnsEmptyCollection()
        {
            List<Phrase> expected = new List<Phrase>();
            expected.Clear(); //expectations
            List<Phrase> method = _viewModel.LoadFromFile("");//loads phrases from the file with empty path parameter
            _viewModel.LoadFromFile(""); //loads phrases from the file with empty path string
            Assert.Empty(_viewModel.LoadedPhrases); // check if LoadedPhrases is empty
            Assert.Equal(expected, method); //compare expectations with method returns
        }
        [Fact]
        public void PopulateDb_GetsEmptyCollectionParameter_DoesNothing()
        {
            _viewModel.LoadedPhrases.Clear(); //collection is empty
            _viewModel.PopulateDb(_viewModel.LoadedPhrases); //PopulateDb with empty collection
            _mainDataProviderMock.Verify(dp => dp.SavePhrase(It.IsAny<Phrase>()), Times.Never); //with empty collection SavePhrase runs never
        }
        [Fact]
        public void LoadFromFile_GetsPathToEmptyFile_ReturnsEmptyCollection()
        {
            List<Phrase> expected = new List<Phrase>();
            expected.Clear(); //expectations
            List<Phrase> method = _viewModel.LoadFromFile("emptyData.csv"); //loads phrases from the file with empty content
            Assert.Empty(_viewModel.LoadedPhrases); // check if LoadedPhrases is empty
            Assert.Equal(expected, method); //compare expectations with method returns
        }
        [Fact]
        public void LoadFromFile_GetsPathToFileWithNotValidCsvData_ReturnsEmptyCollection()
        {
            List<Phrase> expected = new List<Phrase>();
            expected.Clear(); //expectations
            List<Phrase> method = _viewModel.LoadFromFile("notValidData.csv"); //loads phrases from the file with not valid data
            Assert.Empty(_viewModel.LoadedPhrases); // check if LoadedPhrases is empty
            Assert.Equal(expected, method); //compare expectations with method returns
        }

        //TODO:
        //spr czy otwiera się okno grupy, z parametrem nazwy
        //zaprojektowanie widoku edycji frazy
        //widok edycji frazy
        //zapis nowej/edytowanej frazy w bazie (nowy vm, będzie też korzystać metoda edycji)
        //grupa frazy z listy
        //powrót do głównego, jeśli fraza nowa,
    }
}
