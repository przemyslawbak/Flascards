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
        private List<Mock<IPhraseEditViewModel>> _phraseEditViewModelMocks;
        public MainPageViewModelTests()
        {
            _phraseEditViewModelMocks = new List<Mock<IPhraseEditViewModel>>();
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

        private IPhraseEditViewModel CreatePhraseEditViewModel() //method for creating phrase edit VM
        {
            var phraseEditViewModelMock = new Mock<IPhraseEditViewModel>();
            phraseEditViewModelMock.Setup(vm => vm.LoadPhrase(It.IsAny<int>()))
              .Callback<int?>(phraseId =>
              {
                  phraseEditViewModelMock.Setup(vm => vm.Phrase)
            .Returns(new Phrase());
              });
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
        public void ShouldAddNewPhraseAndGivePhraseEditPropertyTrue()
        {
            _viewModel.PhraseEdit = false;
            _viewModel.AddPhraseCommand.Execute(null);
            Assert.True(_viewModel.PhraseEdit);
            _phraseEditViewModelMocks.First().Verify(vm => vm.LoadPhrase(null), Times.Once);
        }
    }
}
