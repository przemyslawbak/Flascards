using Flashcards.DataAccess;
using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.DataProvider
{
    public class MainDataProvider : IMainDataProvider
    {
        private Func<IDataRepository> _dataServiceCreator;
        public MainDataProvider(Func<IDataRepository> dataServiceCreator)
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
        public string GetStreamFromCSV(string filePath)
        {
            using (var dataService = _dataServiceCreator())
            {
                return dataService.GetStreamFromCSV(filePath);
            }
        }
        public void SavePhrase(Phrase phrase)
        {
            using (var dataService = _dataServiceCreator())
            {
                dataService.SavePhrase(phrase);
            }
        }
    }
}
