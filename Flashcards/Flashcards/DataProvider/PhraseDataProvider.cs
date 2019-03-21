using Flashcards.DataAccess;
using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.DataProvider
{
    public class PhraseDataProvider : IPhraseDataProvider
    {
        private readonly Func<IDataRepository> _dataServiceCreator;

        public PhraseDataProvider(Func<IDataRepository> dataServiceCreator) //ctor
        {
            _dataServiceCreator = dataServiceCreator;
        }
        public Phrase GetPhraseById(int id)
        {
            using (var dataService = _dataServiceCreator())
            {
                return dataService.GetPhraseById(id);
            }
        }
    }
}
