using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.DataAccess
{
    public interface IDatabaseRepository : IDisposable
    {
        IEnumerable<Phrase> GetPhrases();
        List<string> GetGroups();
        int SavePhrase(Phrase phrase);
    }
}
