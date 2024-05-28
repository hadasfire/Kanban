using IntroSE.Kanban.Backend.BuisnessLayer;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardDTO
    {
        private BoardController boardController;
        private Boolean isPersisted;
        private string name;
        private List<TaskDTO> tasks;
        private int taskId;
        private int backLogMax;
        private int inProgressMax;
        private int doneMax;
        private UserInProgressTasksDTO inProgressUser;
        private int boardID;
        private string ownerEmail;


        public BoardDTO(string name1, List<TaskDTO> tasks1, int taskId1, int backLogMax1, int inProgressMax1, int doneMax1, Dictionary<string,List<TaskDTO>> inProgressUser1, int boardID1, string ownerEmail1)
        {
            this.boardController = new BoardController();
            this.isPersisted = true;
            this.name = name1;
            this.tasks = tasks1;
            this.taskId = taskId1;
            this.backLogMax = backLogMax1;
            this.inProgressMax = inProgressMax1;
            this.doneMax = doneMax1;
            this.inProgressUser = new UserInProgressTasksDTO(inProgressUser1);
            this.boardID = boardID1;
            this.ownerEmail = ownerEmail1;
        }

        public BoardDTO(UserInProgressTasks u1, string name1, User user1, int boardID1)
        {
            this.boardController = new BoardController();
            this.isPersisted = false;
            this.name = name1;
            this.tasks = new List<TaskDTO>();
            this.taskId = 0;
            this.backLogMax = -1;
            this.inProgressMax = -1;
            this.doneMax = -1;
            this.inProgressUser = u1.UserInProgressTasksDTO;
            this.boardID = boardID1;
            this.ownerEmail = user1.Email;
            persist();
        }
        public void persist()
        {
            try
            {
                boardController.Insert(this);
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
                boardController.Delete(this);
                isPersisted = false;
            }
            catch (KanbanDataException e)
            {
                throw new KanbanDataException("didn't deleted to the data base");
            }
            catch (Exception ex)
            {
                throw new Exception(($"An unexpected error occured: \n {ex.Message} \nplease contact"));
            }
        }



        public List<TaskDTO> Tasks { get => tasks; }


        public string Name { get => name; }
        public int TaskId { get => taskId; }
        internal int BackLogMax { get => backLogMax;
            set
            {
                if (isPersisted)
                    boardController.Update(boardID, "BackLogMax", value);
                backLogMax = value;
            }
        }
        internal int InProgressMax { get => inProgressMax;
            set
            {
                if (isPersisted)
                    boardController.Update(boardID, "InProgressMax", value);
                inProgressMax = value;
            }
        }
        internal int DoneMax { get => doneMax;
            set
            {
                if (isPersisted)
                    boardController.Update(boardID, "DoneMax", value);
                doneMax = value;
            }
        }
        
        
        public UserInProgressTasksDTO InProgressUser { get => inProgressUser; }
        public int BoardId { get => boardID; }
        public string OwnerEmail { get => ownerEmail;
            set
            {
                if (isPersisted)
                    boardController.Update(boardID, "OwnerEmail", value);
                ownerEmail = value;
            }
        }

        public void addTask(TaskDTO task)
        {
            task.BoardID=this.boardID;
            task.persist();
            tasks.Add(task); // if persist failed, it won't reach to this line
        }
    }
}
