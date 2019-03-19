using Flashcards.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.DataProvider
{
    public class MainDataProvider : IMainDataProvider
    {
        private Func<IDatabaseRepository> _dataServiceCreator;
        public MainDataProvider(Func<IDatabaseRepository> dataServiceCreator)
        {
            _dataServiceCreator = dataServiceCreator;
        }
        public List<string> GetGroups()
        {
            using (var dataService = _dataServiceCreator())
            {
                return dataService.GetGroups();
            }
        }
    }
}
