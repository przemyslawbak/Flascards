using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.Models
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
