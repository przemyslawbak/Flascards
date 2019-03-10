using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Flashcards.Models
{
    public class DatabaseRepository
    {
        SQLiteConnection database;
        public DatabaseRepository()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Phrase>();
        }
    }
}
