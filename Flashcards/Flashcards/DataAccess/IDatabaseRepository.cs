using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.DataAccess
{
    public interface IDatabaseRepository : IDisposable
    {
        Phrase GetPhraseById(int id);
        IEnumerable<Phrase> GetPhrases();
        List<string> GetGroups();
        int SavePhrase(Phrase phrase);
    }
}
