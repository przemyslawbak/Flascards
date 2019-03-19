using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.DataProvider
{
    public interface IPhraseDataProvider
    {
        Phrase GetPhraseById(int id);
    }
}
