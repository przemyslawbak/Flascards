using Flashcards.DataProvider;
using Flashcards.Models;
using Flashcards.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Flascards.UnitTests.ViewModels
{
    public class MainPageViewModelTests
    {
        private MainPageViewModel _viewModel;
        private Mock<IPhraseEditViewModel> _phraseEditViewModelMock;
        public MainPageViewModelTests()
        {
            _phraseEditViewModelMock = new Mock<IPhraseEditViewModel>();
            var mainDataProviderMock = new Mock<IMainDataProvider>();
            mainDataProviderMock.Setup(dp => dp.GetGroups())
              .Returns(new List<string>
              {
          "Group #1",
          "Group #2",
          "Group #3"
             });
            mainDataProviderMock.Setup(dp => dp.GetStreamFromCSV("data.csv"))
              .Returns("Name|Definition|Category|Group|Priority\nname1 |def1|cat1|gr1|prio1\nname2 |def2|cat2|gr2|prio2");

            _viewModel = new MainPageViewModel(mainDataProviderMock.Object, CreatePhraseEditViewModel);
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
            var friend = _viewModel.Groups[0];
            Assert.NotNull(friend);
            Assert.Equal("Group #1", friend);
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
        public void LoadFromFileShouldConvertReturnedStringToObjectList()
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
    }
}
