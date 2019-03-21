using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.DataProvider
{
    public interface IMainDataProvider
    {
        List<string> GetGroups();
        string GetStreamFromCSV(string filePath);
        void SavePhrase(Phrase phrase);
        Task<string> PickUpFile();
    }
}
