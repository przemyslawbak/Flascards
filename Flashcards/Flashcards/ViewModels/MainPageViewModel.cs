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
            DatabaseRepository repo = new DatabaseRepository();
            Groups = new List<string>();
            Groups = repo.GetGroups();
        }
    }
}
