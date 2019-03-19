using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.DataProvider
{
    public interface IMainDataProvider
    {
        List<string> GetGroups();
    }
}
