using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Flashcards.Models
{
    public class DatabaseRepository
    {
        SQLiteConnection database;
        static object locker = new object();
        public DatabaseRepository()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Phrase>();
        }
        public IEnumerable<Phrase> GetPhrases()
        {
            lock (locker)
            {
                return (from p in database.Table<Phrase>()
                        select p).ToList();
            }
        }
        public List<string> GetGroups()
        {
            lock (locker)
            {
                List<string> groups = new List<string>();
                var context = database.Table<Phrase>();
                foreach(Phrase phrase in context)
                {
                    groups.Add(phrase.Group);
                }
                return groups.ToList();
            }
        }
        public int SavePhrase(Phrase phrase)
        {
            lock (locker)
            {
                if (phrase.Id != 0)
                {
                    database.Update(phrase);
                    return phrase.Id;
                }
                else
                {
                    return database.Insert(phrase);
                }
            }
        }
    }
}
