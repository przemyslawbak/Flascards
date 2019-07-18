using Flashcards.DataProvider;
using Flashcards.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Flashcards.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GroupPage : ContentPage
    {
        private IEventAggregator _eventAggregator; //Prism
        private GroupPageViewModel _viewModel;
        public GroupPage(string groupName, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator; //Prism
            InitializeComponent();
            _viewModel = new GroupPageViewModel(groupName, _eventAggregator);
            BindingContext = _viewModel;
		}
	}
}