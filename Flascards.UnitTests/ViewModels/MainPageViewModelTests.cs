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
        public void ShouldLoadGroupsOnlyOnce()
        {
            _viewModel.LoadGroups();
            _viewModel.LoadGroups();

            Assert.Equal(3, _viewModel.Groups.Count);
        }
        [Fact]
        public void ShouldLoadGroups()
        {
            _viewModel.LoadGroups();
            Assert.Equal(3, _viewModel.Groups.Count);
            var phrase = _viewModel.Groups[0];
            Assert.NotNull(phrase);
            Assert.Equal("Group #1", phrase);
        }
        [Fact]
        public void ShouldOpenNewPhraseAndSetPhraseEditPropertyTrue()
        {
            _viewModel.PhraseEdit = false;
            _viewModel.AddPhraseCommand.Execute(null);
            Assert.True(_viewModel.PhraseEdit);
            _phraseEditViewModelMock.Verify(vm => vm.LoadPhrase(null), Times.Once);
        }
        [Fact]
        public void LoadFromFileMethodShouldConvertReturnedCorrectStringToObjectList()
        {
            _viewModel.LoadFromFile("data.csv");
            Assert.Equal(2, _viewModel.LoadedPhrases.Count);
            var phrase = _viewModel.LoadedPhrases[0];
            Assert.NotNull(phrase);
            Assert.Equal("name1", phrase.Name);
            Assert.Equal("def1", phrase.Definition);
            Assert.Equal("cat1", phrase.Category);
            Assert.Equal("gr1", phrase.Group);
            Assert.Equal("prio1", phrase.Priority);
        }
        [Fact]
        public void PopulateDbMethodShouldSavePhraseOfDataProvider()
        {
            _viewModel.PopulateDb(phrases);
            _mainDataProviderMock.Verify(dp => dp.SavePhrase(phrases[0]), Times.Once);
        }

        [Fact]
        public void LoadFileCommandExecuteFiresOnLoadFileExecute()
        {
            _viewModel.LoadFile.Execute(null);
            Assert.Equal(2, _viewModel.LoadedPhrases.Count());
            Assert.Equal(3, _viewModel.Groups.Count);
            //savephrase przejście przez własność w VM, może, albo nie wiem
            _mainDataProviderMock.Verify(dp => dp.SavePhrase(phrases[0]), Times.Once);
        }

        //TODO:
        //zły format pliku
        //brak pliku
        //LoadedPhrases ładuje raz
        //PopulateDb łąduje raz
    }
}
