using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.DataAccess
{
    /// <summary>
    /// Interface for data access repository from SQLite and CSV
    /// </summary>
    public interface IDataRepository : IDisposable
    {
        Phrase GetPhraseById(int id);
        IEnumerable<Phrase> GetPhrases();
        List<string> GetGroups();
        void SavePhrase(Phrase phrase);
        string GetStreamFromCSV(string filePath);
        Task<string> PickUpFile();
    }
}
