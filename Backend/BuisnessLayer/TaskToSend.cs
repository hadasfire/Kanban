using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    public class TaskToSend
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        [JsonConstructor]
        public TaskToSend(int Id, DateTime CreationTime, string Title, string Description, DateTime DueDate)
        {
            this.Id = Id;
            this.CreationTime = CreationTime;
            this.Title = Title;
            this.Description = Description;
            this.DueDate = DueDate;
        }

        internal TaskToSend(Task task)
        {
            Id = task.ID;
            CreationTime = task.CreationTime;
            Title = task.Title;
            Description = task.Description;
            DueDate = task.DueDate;
        }

        internal TaskToSend()
        {
            this.Id = -1;
            this.CreationTime = DateTime.Now;
            this.Title = string.Empty;
            this.Description = string.Empty;
            this.DueDate = DateTime.Now;
        }
    }
}
