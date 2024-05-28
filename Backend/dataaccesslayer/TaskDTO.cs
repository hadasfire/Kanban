using IntroSE.Kanban.Backend.BuisnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskDTO
    {
        private TaskController taskController;
        private Boolean isPersisted;
        private int id;
        private DateTime creationTime;
        private DateTime dueDate;
        private string title;
        private string description;
        private string emailAssignee;
        private int boardID;
        private int columnOrdinal;


        public TaskDTO(int id, DateTime creationTime, DateTime dueDate, string title, string description)
        {
            this.taskController = new TaskController();
            isPersisted = false;
            this.id = id;
            this.creationTime = creationTime;
            this.dueDate = dueDate;
            this.title = title;
            this.description = description;
            this.emailAssignee = null;
            this.boardID = -1;
            this.columnOrdinal = 0;
        }

        public TaskDTO(int id, DateTime creationTime, DateTime dueDate, string title, string description, string emailAssignee, int boardID, int columnOrdinal)
        {
            this.taskController = new TaskController();
            isPersisted = true;
            this.id = id;
            this.creationTime = creationTime;
            this.dueDate = dueDate;
            this.title = title;
            this.description = description;
            this.emailAssignee = emailAssignee;
            this.boardID = boardID;
            this.columnOrdinal = columnOrdinal;
        }

        internal int ColumnOrdinal
        {
            get => columnOrdinal;
            set
            {
                if (isPersisted)
                {
                    taskController.Update(id, "Column", value);
                }
                columnOrdinal = value;
            }
        }

        internal string Title
        {
            get => title;
            set
            {
                if (isPersisted)
                {
                    taskController.Update(id, "Title", value);
                }
                title = value;
            }
        }

        internal DateTime DueDate
        {
            get => dueDate;
            set
            {
                if (isPersisted)
                {
                    taskController.Update(id, "DueDate", value);
                }
                dueDate = value;
            }
        }


        internal string Description
        {
            get => description;
            set
            {
                if (isPersisted)
                {
                    taskController.Update(id, "Title", value);
                }
                description = value;
            }
        }

        internal DateTime CreationTime
        {
            get => creationTime;
        }

        internal int Id
        {
            get => id;
        }

        internal string EmailAssignee
        {
            get => emailAssignee;
            set
            {
                if (isPersisted)
                {
                    taskController.Update(id, "EmailAssignee", value);
                }
                emailAssignee = value;
            }
        }


        internal int BoardID
        {
            get => boardID;
            set
            {
                boardID = value;
            }
        }

        public void persist()
        {
            try
            {
                taskController.Insert(this);
                isPersisted = true;
            }
            catch (KanbanDataException e)
            {
                throw new KanbanDataException("didn't added to the data base");
            }
            catch (Exception ex)
            {
                throw new Exception(($"An unexpected error occured: \n {ex.Message} \nplease contact"));
            }
        }

        public void delete()
        {
            try
            {
                taskController.Delete(this);
                isPersisted = false;
            }
            catch (KanbanDataException e)
            {
                throw new KanbanDataException("didn't added to the data base");
            }
            catch (Exception ex)
            {
                throw new Exception(($"An unexpected error occured: \n {ex.Message} \nplease contact"));
            }
        }
    }
}



