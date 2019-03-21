using Flashcards.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Flashcards
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel;
        public MainPage(MainPageViewModel viewModel)
        {
            _viewModel = viewModel; //dependency
            _viewModel.LoadGroups(); //calls group load method from the VM
            _viewModel.LoadFromFile("data.csv");
            BindingContext = _viewModel; //sets up view context
            InitializeComponent();
        }
    }
}
