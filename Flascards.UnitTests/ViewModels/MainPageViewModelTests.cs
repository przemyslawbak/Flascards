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
        public void ShouldRunLoadFromFileCommand()
        {

        }
    }
}
