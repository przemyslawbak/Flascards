using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flashcards.Events
{
    public class OpenGroupPageEvent : PubSubEvent<string> //group name parameter
    {
    }
}
