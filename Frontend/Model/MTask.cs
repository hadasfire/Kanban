using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BuisnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class MTask
    {
        private int _id;
        private DateTime _creationTime;
        private string _title;
        private string _description;
        private DateTime _dueDate;

        public int Id
        {
            get { return _id; }
        }

        public DateTime CreationTime
        {
            get { return _creationTime; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Description
        {
            get { return _description; }
        }

        public DateTime DueDate
        {
            get { return _dueDate; }
        }

        /// <summary>
        /// Constructs an new MTask with the given parameters.
        /// </summary>
        /// <param name="id">Task's ID</param>
        /// <param name="creationTime">Task's creation time</param>
        /// <param name="title">Task's title</param>
        /// <param name="description">Task's description</param>
        /// <param name="dueDate">Task's due date</param>
        public MTask(int id, DateTime creationTime, string title, string description, DateTime dueDate)
        {
            this._id = id;
            this._creationTime = creationTime; 
            this._title = title;
            this._description = description;
            this._dueDate = dueDate;
        }

        /// <summary>
        /// Constructs new MTask from the given TaskToSend.
        /// </summary>
        /// <param name="task">A task in TaskToSend format</param>
        public MTask(TaskToSend task)
        {
            this._id = task.Id;
            this._creationTime = task.CreationTime;
            this._title = task.Title;
            this._description = task.Description;
            this._dueDate = task.DueDate;
        }
    }
}
