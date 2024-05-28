using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    internal class KanbanException : Exception
    {
        public KanbanException() { }
        public KanbanException(string message) : base(message)
        { }

        public KanbanException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
