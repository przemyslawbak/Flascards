using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.Models
{
    public class Phrase
    {
        [PrimaryKeyAttribute, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Category { get; set; }
        public string Group { get; set; }
        public string Priority { get; set; }
        public bool Learned { get; set; }
    }
}
