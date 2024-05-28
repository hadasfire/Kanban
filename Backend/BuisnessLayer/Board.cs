using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using log4net;
using log4net.Config;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class Board
{
    private BoardDTO bdto;
    private string name;
    private Dictionary<int, Task> backLogTasks;
    private Dictionary<int, Task> inProgressTasks;
    private Dictionary<int, Task> doneTasks;
    private int TaskId;
    private int backLogMax;
    private int inProgressMax;
    private int doneMax;
    private int boardID;
    private string ownerEmail;
    private UserInProgressTasks inProgressUser;


    public Board(UserInProgressTasks u, string name, User user, int BoardID)
    {
        this.bdto = new BoardDTO(u, name, user, BoardID); 
        this.name = name;
        backLogTasks = new Dictionary<int, Task>();
        inProgressTasks = new Dictionary<int, Task>();
        doneTasks = new Dictionary<int, Task>();
        this.TaskId = 0;
        backLogMax = -1;
        inProgressMax = -1;
        doneMax = -1;
        InProgressUser = u;
        boardID = BoardID;
        ownerEmail = user.Email;
    }

    public Board(BoardDTO bdto)
    {
        this.bdto = bdto;
        this.name = bdto.Name;
        this.backLogTasks = new Dictionary<int, Task>();
        this.inProgressTasks = new Dictionary<int, Task>();
        this.doneTasks = new Dictionary<int, Task>();
        this.TaskId = bdto.TaskId;
        this.backLogMax = bdto.BackLogMax;
        this.inProgressMax = bdto.InProgressMax;
        this.doneMax = bdto.DoneMax;
        this.inProgressUser = new UserInProgressTasks(bdto.InProgressUser);
        this.boardID = bdto.BoardId;
        this.ownerEmail = bdto.OwnerEmail;
        foreach (TaskDTO t in bdto.Tasks)
        {
            if (t.ColumnOrdinal == 0)
            {
                backLogTasks.Add(t.Id, new Task(t));
            }
            if (t.ColumnOrdinal == 1)
            {
                inProgressTasks.Add(t.Id, new Task(t));
            }
            if (t.ColumnOrdinal == 2)
            {
                doneTasks.Add(t.Id, new Task(t));
            }
        }
    }

    public string Name { get =>  name; set => name = value; }
    public int BoardID { get => boardID;}
    public BoardDTO Bdto { get => bdto; }
    public string OwnerEmail { get => ownerEmail; set => ownerEmail = value; }

    public Dictionary<int, Task> BackLogTasks {  get => backLogTasks; set => backLogTasks = value; }
    public Dictionary<int, Task> InProgressTasks { get => inProgressTasks; set => inProgressTasks = value; }
    public Dictionary<int,Task> DoneTasks { get => doneTasks; set => doneTasks = value; }   
    public int BackLogMax { get => backLogMax; set => backLogMax = value; }
    public int DoneMax { get => doneMax; set => doneMax = value; }
    public UserInProgressTasks InProgressUser { get => inProgressUser; set => inProgressUser = value; }

    public Task getTask(int taskId)
    {
        return backLogTasks[taskId];
    }

    public void AddTask(User user, DateTime dueDate, string title, string description, DateTime creationTime)
    {
        if (backLogTasks.Count() == backLogMax)
        {
            throw new KanbanException("The Back Log column is full");
        }
        if (!user.Boards.ContainsKey(name))
            throw new KanbanException("the user is not a member.");
        else
        {
            Task toAdd = new Task(TaskId, creationTime, dueDate, title, description);
            backLogTasks.Add(TaskId, toAdd);
            TaskId = TaskId+1; //uniqe id
            try
            {
                bdto.addTask(toAdd.Tdto);
            }
            catch (Exception ex)
            {
                backLogTasks.Remove(TaskId);
                throw new Exception (ex.Message);
            }
        }
    }

    public void LimitColumn(int columnOrdinal, int max)
    {
        if (columnOrdinal==0)
        {
            backLogMax = max;
            bdto.BackLogMax=max;
        }
        else if (columnOrdinal == 1)
        {
            inProgressMax = max;
            bdto.InProgressMax=max;
        }
        else 
        {
            doneMax = max;
            bdto.DoneMax=max;
        }
    }

    public int GetColumnLimit(int columnOrdinal)
    {
        if (columnOrdinal == 0)
        {
            return backLogMax;
        }
        else if (columnOrdinal == 1)
        {
            return inProgressMax;
        }
        else
        {
            return doneMax;
        }
    }
     
    public string GetColumnName(int columnOrdinal)
    {
        if (columnOrdinal == 0)
        {
            return "backlog";
        }
        else if (columnOrdinal == 1)
        {
            return "in progress";
        }
        else
        {
            return "done";
        }

    }

    public List<Task> GetColumn(int columnOrdinal)
    {
        if (columnOrdinal == 0)
        {
            List<Task> list1 = backLogTasks.Values.ToList();
            return list1;
        }
        else if (columnOrdinal == 1)
        {
            List<Task> list2 = inProgressTasks.Values.ToList();
            return list2;
        }
        else if (columnOrdinal == 2)
        {
            List<Task> list3 = doneTasks.Values.ToList();
            return list3;
        }
        else
        {
            throw new KanbanException("Invalid column ordinal");
        }
    }


    public List<TaskToSend> GetColumnToSend(int columnOrdinal)
    {
        if (columnOrdinal == 0)
        {
            List<Task> list = backLogTasks.Values.ToList();
            List<TaskToSend> output = new List<TaskToSend>();
            foreach (Task t in list)
            {
                TaskToSend t1 = new TaskToSend(t);
                output.Add(t1);
            }
            return output;
        }
        else if (columnOrdinal == 1)
        {
            List<Task> list = inProgressTasks.Values.ToList();
            List<TaskToSend> output = new List<TaskToSend>();
            foreach (Task t in list)
            {
                TaskToSend t1 = new TaskToSend(t);
                output.Add(t1);
            }
            return output;
        }
        else if (columnOrdinal == 2)
        {
            List<Task> list = doneTasks.Values.ToList();
            List<TaskToSend> output = new List<TaskToSend>();
            foreach (Task t in list)
            {
                TaskToSend t1 = new TaskToSend(t);
                output.Add(t1);
            }
            return output;
        }
        else
        {
            throw new KanbanException("Invalid column ordinal");
        }
    }


    public void UpdateTaskDueDate(string email, int taskId, int columnOrdinal, DateTime dueDate)
    {
        if (columnOrdinal==0) {
            if (!backLogTasks.ContainsKey(taskId))
            {
                throw new KanbanException("Invalid taskId");
            }
            if (backLogTasks[taskId].EmailAssignee!=null && !backLogTasks[taskId].EmailAssignee.Equals(email))
            {
                throw new KanbanException("User is not assignee");
            }
            backLogTasks[taskId].DueDate = dueDate;
        }
        else if (columnOrdinal==1) {
            if (!inProgressTasks.ContainsKey(taskId))
            {
                throw new KanbanException("Invalid taskId");
            }
            if (inProgressTasks[taskId].EmailAssignee != null && !inProgressTasks[taskId].EmailAssignee.Equals(email))
            {
                throw new KanbanException("User is not assignee");
            }
            inProgressTasks[taskId].DueDate= dueDate;
        }
        else {
            throw new KanbanException("Invalid taskId");
        }
    }

    public void UpdateTaskTitle(string email, int taskId, int columnOrdinal, string title)
    {
        if (title.Length < 0 || title.Length > 50)
        {
            throw new KanbanException("Invalid title- max 50 characters.");
        }
        if (columnOrdinal == 0)
        {
            if (!backLogTasks.ContainsKey(taskId))
            {
                throw new KanbanException("Invalid taskId");
            }
            if (backLogTasks[taskId].EmailAssignee!=null && !backLogTasks[taskId].EmailAssignee.Equals(email))
            {
                throw new KanbanException("User is not assignee");
            }
            backLogTasks[taskId].Title= title;
        }
        else if (columnOrdinal == 1)
        {
            if (!inProgressTasks.ContainsKey(taskId))
            {
                throw new KanbanException("Invalid taskId");
            }
            if (inProgressTasks[taskId].EmailAssignee !=null && !inProgressTasks[taskId].EmailAssignee.Equals(email))
            {
                throw new KanbanException("User is not assignee");
            }
            inProgressTasks[taskId].Title=title;
        }
        else
        {
            throw new KanbanException("Invalid taskId");
        }
    }

    public void UpdateTaskDescription(string email, int taskId, int columnOrdinal, string description)
    {
        if (description.Length > 300)
        {
            throw new KanbanException("Invalid description- max 300 characters.");
        }
        else if (columnOrdinal == 0)
        {
            if (!backLogTasks.ContainsKey(taskId))
            {
                throw new KanbanException("Invalid taskId");
            }
            if (backLogTasks[taskId].EmailAssignee != null && !backLogTasks[taskId].EmailAssignee.Equals(email))
            {
                throw new KanbanException("User is not assignee");
            }
            backLogTasks[taskId].Description= description;
        }
        else if (columnOrdinal == 1)
        {
            if (!inProgressTasks.ContainsKey(taskId))
            {
                throw new KanbanException("Invalid taskId");
            }
            if (inProgressTasks[taskId].EmailAssignee != null && !inProgressTasks[taskId].EmailAssignee.Equals(email))
            {
                throw new KanbanException("User is not assignee");
            }
            inProgressTasks[taskId].Description= description;
        }
        else
        {
                throw new KanbanException("Invalid taskId");
        }
    }

    public void AdvanceTask(string email, int columnOrdinal, int taskId)
    {
        if (columnOrdinal == 0)
        {
            if (!backLogTasks.ContainsKey(taskId))
            {
                throw new KanbanException("Invalid taskId");
            }
            if (inProgressTasks.Count() == inProgressMax)
            {
                throw new KanbanException("'In progress' column is full");
            }
            if (backLogTasks[taskId].EmailAssignee != null && !backLogTasks[taskId].EmailAssignee.Equals(email))
            {
                throw new KanbanException("User is not assignee");
            }
            else
            {
                inProgressTasks.Add(taskId, backLogTasks[taskId]);
                inProgressUser.AddTasks(email, backLogTasks[taskId]);
                backLogTasks.Remove(taskId);
                Task task = inProgressTasks[taskId];
                task.Tdto.ColumnOrdinal = 1;
            }
        }
        else if (columnOrdinal == 1)
        {
            if (!inProgressTasks.ContainsKey(taskId))
            {
                throw new KanbanException("Invalid taskId");
            }
            if (doneTasks.Count() == doneMax)
            {
                throw new KanbanException("'Done' column is full");
            }
            if (inProgressTasks[taskId].EmailAssignee != null && !inProgressTasks[taskId].EmailAssignee.Equals(email))
            {
                throw new KanbanException("User is not assignee");
            }
            else
            {
                doneTasks.Add(taskId, inProgressTasks[taskId]);
                inProgressTasks.Remove(taskId);
                Task task = doneTasks[taskId];
                inProgressUser.RemoveTasks(email, task);
                task.Tdto.ColumnOrdinal = 2;
            }
        }
        else
            throw new KanbanException("Not possible");
        }


    public void TransferOwnership(User newOwner)
    {
        ownerEmail = newOwner.Email;
        bdto.OwnerEmail = newOwner.Email;
    }

    public void AssignTask(string email, int columnOrdinal, int taskID, string AssigneeEmail)
    {
        if (columnOrdinal == 0)
        {
            if (!backLogTasks.ContainsKey(taskID))
            {
                throw new KanbanException("Invalid taskId");
            }
            if (backLogTasks[taskID].EmailAssignee != null && !backLogTasks[taskID].EmailAssignee.Equals(email))
            {
                throw new KanbanException("User that is not assignee canno't change assignee");
            }
            backLogTasks[taskID].EmailAssignee = AssigneeEmail;
            }
        if (columnOrdinal == 1)
        {
            if (!inProgressTasks.ContainsKey(taskID))
            {
                throw new KanbanException("Invalid taskId");
            }
            if (backLogTasks[taskID].EmailAssignee != null && !backLogTasks[taskID].EmailAssignee.Equals(email))
            {
                throw new KanbanException("User that is not assignee canno't change assignee");
            }
            inProgressTasks[taskID].EmailAssignee = AssigneeEmail;
            inProgressUser.AddTasks(AssigneeEmail, inProgressTasks[taskID]);
            if (backLogTasks[taskID].EmailAssignee != null)
            {
                inProgressUser.RemoveTasks(email, inProgressTasks[taskID]);
            }
            }

        }
    }









