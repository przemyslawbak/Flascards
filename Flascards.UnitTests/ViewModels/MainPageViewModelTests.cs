using Flashcards.DataProvider;
using Flashcards.Models;
using Flashcards.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Flascards.UnitTests.ViewModels
{
    public class MainPageViewModelTests
    {
        private MainPageViewModel _viewModel;

        public MainPageViewModelTests()
        {
            var mainDataProviderMock = new Mock<IMainDataProvider>();
            mainDataProviderMock.Setup(dp => dp.GetGroups())
              .Returns(new List<string>
              {
          "Group #1",
          "Group #2",
          "Group #3"
             });
            _viewModel = new MainPageViewModel(mainDataProviderMock.Object);
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
        public void ShouldOpenNewPageOnNewPhraseExecute()
        {
            Phrase phrase = new Phrase();
            _viewModel.NewPhraseCommand.Execute(phrase);
            Assert.True(_viewModel.NewPhraseEdit);
        }
    }
}
