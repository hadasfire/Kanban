using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    internal class KanbanDataException : Exception
    {
        public KanbanDataException() { }
        public KanbanDataException(string message) : base(message)
        { }

        public KanbanDataException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}