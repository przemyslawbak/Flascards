using Flashcards.DataAccess;
using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.DataProvider
{
    public class PhraseDataProvider
    {
        private readonly Func<IDatabaseRepository> _dataServiceCreator;

        public PhraseDataProvider(Func<IDatabaseRepository> dataServiceCreator) //ctor
        {
            _dataServiceCreator = dataServiceCreator;
        }
        //implementacja interfejsu
        public Phrase GetPhraseById(int id)
        {
            using (var dataService = _dataServiceCreator())
            {
                return dataService.GetPhraseById(id);
            }
        }
    }
}
