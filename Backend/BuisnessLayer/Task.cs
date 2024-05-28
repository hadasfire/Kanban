using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System;

public class Task
{
    private TaskDTO tdto;
    private int id;
    private DateTime creationTime;
    private DateTime dueDate;
    private string title;
    private string description;
    private string emailAssignee;

    public Task(int id, DateTime creationTime, DateTime dueDate, string title, string description)
    {
        this.tdto = new TaskDTO(id, creationTime, dueDate, title, description);
        this.id = id;
        this.creationTime = creationTime;
        this.dueDate = dueDate;
        Title = title;
        Description = description;
        this.emailAssignee = null;
    }

    public Task(TaskDTO td)
    {
        this.tdto = td;
        this.id = td.Id;
        this.creationTime = td.CreationTime;
        this.dueDate=td.DueDate; 
        this.title = td.Title;
        this.description = td.Description;
        this.emailAssignee = td.EmailAssignee;
    }

    public TaskDTO Tdto { get => tdto; }
    public int ID { get => id; set => id = value; }
    public DateTime CreationTime { get => creationTime; }

    internal string Title
    {
        get => title;
        set
        {
            if (value == null || value.Length > 50 || value.Length < 1)
                throw new KanbanException("Title is null or has more than 50 characters");
            else
            {
                title = value;
                tdto.Title = value;
            }
        }
    }
    internal string Description { get => description;
        set
        {
            if (value == null || value.Length > 300)
                throw new KanbanException("Description is too long or null");
            else
            {
                description = value;
                tdto.Description = value;
            }
        }
    }

    internal DateTime DueDate
    {
        get => dueDate;
        set
        {
            dueDate = value;
            tdto.DueDate = value;  
        }
    }

    public string EmailAssignee
    {
        get => emailAssignee;
        set
        {
            emailAssignee = value;
            tdto.EmailAssignee = value;    
        }
    }


    public int GetId() { 
        return id; 
    }


    public void AssignTask(string email, string boardName, int columnOrdinal, int TaskId, string emailAssignee)
    {
        EmailAssignee = emailAssignee;
    }
}
