using Flashcards.DataProvider;
using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.ViewModels
{
    public interface IPhraseEditViewModel
    {
        void LoadPhrase(int? friendId);
    }
    public class PhraseEditViewModel : IPhraseEditViewModel
    {
        private IPhraseDataProvider _dataProvider;
        public PhraseEditViewModel(IPhraseDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        public void LoadPhrase(int? phraseId)
        {
            var phrase = phraseId.HasValue
              ? _dataProvider.GetPhraseById(phraseId.Value)
              : new Phrase();
        }
    }
}
