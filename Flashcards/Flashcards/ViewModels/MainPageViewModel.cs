using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.ViewModels
{
    public class MainPageViewModel
    {
        public List<string> Groups { get; set; }
        public MainPageViewModel()
        {
            Groups = new List<string>();
            Groups = DatabaseRepository.GetGroups();
        }
    }
}
