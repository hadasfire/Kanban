using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

public class UserInProgressTasks
{
	UserInProgressTasksDTO userInProgressTasksDTO;
	private Dictionary<string, List<Task>> userTasks;

    public Dictionary<string, List<Task>> UserTasks { get => userTasks; }

    public UserInProgressTasks() 
	{
        userInProgressTasksDTO = new UserInProgressTasksDTO();
		userTasks = new Dictionary<string, List<Task>>();
    }

    public UserInProgressTasksDTO Name { get => userInProgressTasksDTO; }

    public UserInProgressTasks(UserInProgressTasksDTO tasks)
    {
        userTasks = new Dictionary<string, List<Task>>();
		foreach (string email in tasks.UserTasks.Keys)
		{
			userTasks.Add(email, new List<Task>());
			foreach (TaskDTO taskDTO in tasks.UserTasks[email])
			{
				userTasks[email].Add(new Task(taskDTO));
            }
		}
    }

	public UserInProgressTasksDTO UserInProgressTasksDTO
	{
		get => userInProgressTasksDTO;
	}

    public void AddUser(string email)
	{
        if (!userTasks.ContainsKey(email))
		{
            userTasks.Add(email, new List<Task>());
			userInProgressTasksDTO.AddUser(email);
        }
    }
	public void AddTasks(string email, Task task)
	{
		if (task == null)
		{
			throw new KanbanException("Task in null");
		}
		userTasks[email].Add(task);
		userInProgressTasksDTO.addTasks(email, task.Tdto);
	}
	public void RemoveTasks(string email, Task task)
	{
		if (task == null)
		{
            throw new KanbanException("Task in null");
        }
        userTasks[email].Remove(task);
        userInProgressTasksDTO.UserTasks[email].Remove(task.Tdto);

    }
    public List<Task> GetList(string email) 
	{
		if (userTasks[email] == null)
		{
			throw new KanbanException("No Tasks in progress");
		}
		return userTasks[email];
	}

	public void addAllTasks(string email, List<Task> tasks)
	{
		userTasks[email] = tasks;
	}
}
