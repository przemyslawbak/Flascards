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
        public static List<string> GetGroups()
        {
            lock (locker)
            {
                List<string> groups = new List<string>();
                var context = SeedDb.CreatePhrases(); //seeding list, CHANGE!
                foreach(Phrase phrase in context)
                {
                    groups.Add(phrase.Group);
                }
                return groups;
            }
        }
    }
}
