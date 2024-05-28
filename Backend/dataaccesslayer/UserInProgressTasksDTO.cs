using IntroSE.Kanban.Backend.BuisnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserInProgressTasksDTO
    { 
        private Dictionary<string, List<TaskDTO>> userTasks;
        private TaskController taskController;
        public UserInProgressTasksDTO(Dictionary<string, List<TaskDTO>> tasks) 
        {
            userTasks = tasks;
        }

        public UserInProgressTasksDTO()
        {
            userTasks = new Dictionary<string, List<TaskDTO>>();
        }

        internal Dictionary<string, List<TaskDTO>> UserTasks
        { get => userTasks; }

        public void AddUser(string email)
        {
            if (!userTasks.ContainsKey(email))
            {
                userTasks.Add(email, new List<TaskDTO>());
            }
        }
        public void addTasks(string email, TaskDTO task)
        {
            userTasks[email].Add(task);
        }
    }
}
