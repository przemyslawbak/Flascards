using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
        int SavePhrase(Phrase phrase);
        string GetStreamFromCSV(string filePath);
    }
}
