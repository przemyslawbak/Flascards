using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.Models
{
    public class SeedDb
    {
        public static List<Phrase> CreatePhrases()
        {
            List<Phrase> phrases = new List<Phrase>();
            Phrase phrase = new Phrase();
            phrase.Category = "testCategory";
            phrase.Definition = "testDefinition";
            phrase.Group = "group1";
            phrase.Id = 1;
            phrase.Learned = false;
            phrase.Name = "testName";
            phrase.Priority = "testPriority";
            phrases.Add(phrase);
            return phrases;
        }
    }
}
