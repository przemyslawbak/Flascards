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
                .ReturnsAsync("data.csv");
            _mainDataProviderMock.Setup(dp => dp.GetStreamFromCSV("data.csv"))
                 .Returns("Name|Definition|Category|Group|Priority\nname1 |def1|cat1|gr1|prio1\nname2 |def2|cat2|gr2|prio2");

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
        public void LoadGroupsMethod_ShouldLoadOnce_True()
        {
            _viewModel.LoadGroups(); //loads groups twice
            _viewModel.LoadGroups();

            Assert.Equal(3, _viewModel.Groups.Count); //counts how many groups are loaded
        }
        [Fact]
        public void LoadGroupsMethod_ShouldLoad_True()
        {
            _viewModel.LoadGroups(); //loads collection of groups (from setup)
            Assert.Equal(3, _viewModel.Groups.Count); //counts groups
            var phrase = _viewModel.Groups[0];
            Assert.NotNull(phrase);
            Assert.Equal("Group #1", phrase); //compares group name
        }
        [Fact]
        public void AddPhraseCommand_ShouldBeExecuted_True()
        {
            _viewModel.PhraseEdit = false; //set up PhraseEdit prop
            _viewModel.AddPhraseCommand.Execute(null); // executes command
            Assert.True(_viewModel.PhraseEdit); //verifies PhraseEdit prop
            _phraseEditViewModelMock.Verify(vm => vm.LoadPhrase(null), Times.Once); //counts loaded phrases
        }
        [Fact]
        public void LoadFromFileMethod_ShouldConvertReturnedCorrectFormatString_ReturnsPhraseList()
        {
            _viewModel.LoadFromFile("data.csv"); //loads phrases from the file
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
        public void PopulateDbMethod_ShouldSeedDbWithPhrases_CallsDpSavePhrase()
        {
            _viewModel.LoadedPhrases = phrases; //populates collection
            _viewModel.PopulateDb(_viewModel.LoadedPhrases); //populates Db with phase list - 1 item
            _mainDataProviderMock.Verify(dp => dp.SavePhrase(It.IsAny<Phrase>()), Times.Once); //counts saved phrases
        }

        [Fact]
        public void LoadFileCommand_ShouldBeExecuted_CallsOnLoadFileExecute()
        {
            _viewModel.LoadFile.Execute(null); //execute command
            Assert.Equal(2, _viewModel.LoadedPhrases.Count()); //counts loaded phrases from the file
            Assert.Equal(3, _viewModel.Groups.Count); //counts loaded groups
            _mainDataProviderMock.Verify(dp => dp.SavePhrase(It.IsAny<Phrase>()), Times.AtLeast(2)); //counts saved phrases
        }
        [Fact]
        public void PopulateDbMethod_ShouldSeedDbOnce_True()
        {
            _viewModel.LoadedPhrases = phrases; //populates collection
            _viewModel.PopulateDb(phrases); //seeds Db twice
            _viewModel.PopulateDb(phrases);
            _mainDataProviderMock.Verify(dp => dp.SavePhrase(It.IsAny<Phrase>()), Times.Once); //should seed only once
        }

        //TODO:
        //zły format pliku
        //brak pliku
        //LoadedPhrases ładuje raz
        //PopulateDb łąduje raz
    }
}
