using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.DataAccess
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
