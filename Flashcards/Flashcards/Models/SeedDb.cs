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
            Phrase phrase2 = new Phrase();
            phrase2.Category = "testCategory";
            phrase2.Definition = "testDefinition";
            phrase2.Group = "group2";
            phrase2.Id = 2;
            phrase2.Learned = false;
            phrase2.Name = "testName";
            phrase2.Priority = "testPriority";
            phrases.Add(phrase2);
            return phrases;
        }
    }
}
