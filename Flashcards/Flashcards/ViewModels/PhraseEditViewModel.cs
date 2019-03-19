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
        Phrase Phrase { get; }
    }
    public class PhraseEditViewModel : ViewModelBase, IPhraseEditViewModel
    {
        private Phrase _phrase;
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
        public Phrase Phrase
        {
            get
            {
                return _phrase; //zwraca fumfla z wrappera
            }
            set
            {
                _phrase = value; //ustawia fumwla przez wrappera
                OnPropertyChanged();
            }
        }
    }
}
