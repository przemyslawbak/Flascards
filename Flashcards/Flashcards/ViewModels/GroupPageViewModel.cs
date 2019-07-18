using Flashcards.DataProvider;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.ViewModels
{
    public interface IGroupPageViewModel
    {
        void LoadGroupName(string groupName);
        string GroupName { get; }
    }
    public class GroupPageViewModel : ViewModelBase, IGroupPageViewModel
    {
        private IEventAggregator _eventAggregator; //Prism
        private string _groupName;
        public GroupPageViewModel()
        {
        }
        public GroupPageViewModel(string groupName, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator; //Prism
            LoadGroupName(groupName);
        }
        public void LoadGroupName(string groupName)
        {
            GroupName = groupName;
        }
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
                OnPropertyChanged();
            }
        }
    }
}
