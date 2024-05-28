using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

public class BoardFacade
{
    private Dictionary<int, string> idToNameBoards;// dictionary of id and name for each board
    private Dictionary<string, Board> boards; // dictionary of uniqe boardname and boards
	private UserFacade uf;
	private UserInProgressTasks inProgressUser;
    private int BoardID;

	public BoardFacade(UserFacade uF)
	{
		this.boards = new Dictionary<string, Board>();
		this.uf = uF;
        this.inProgressUser = new UserInProgressTasks();
        this.BoardID = 0;
        idToNameBoards = new Dictionary<int, string>();
	}
	public void CreateBoard(string email, string boardName)
	{
		User user = uf.GetUser(email);
        if (!user.IsLoggedIn()) 
        {
            throw new KanbanException("User is not logged in");
        }
        if (boards.ContainsKey(boardName)) 
        {
            throw new KanbanException("this board name already exists");
        }
        Board board = new Board(inProgressUser, boardName, user, BoardID);
        boards.Add(boardName, board);
        idToNameBoards.Add(board.BoardID, boardName);
        user.AddUserToBoard(board);
        inProgressUser.AddUser(email);
        BoardID = BoardID + 1;
    }
    public void DeleteBoard(string email, string boardName)
    {
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("User is not logged in");
        }
        if (!boards[boardName].OwnerEmail.Equals(email))
        {
            throw new KanbanException("User is not owner");
        }
        if (!boards.ContainsKey(boardName))
        {
            throw new KanbanException("This board name does not exists");
        }
        idToNameBoards.Remove(boards[boardName].BoardID);
        uf.DeleteBoardFromAllUsers(boards[boardName]);
        boards.Remove(boardName);
    }
    public void AddTask(string email,string boardName, string title, string description, DateTime dueDate, DateTime creationTime)
	{
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("User is not logged in");
        }
        Board board = boards[boardName];
        if (board == null)
        {
            throw new KanbanException("This board name does not exists");
        }
        if (!user.Boards.ContainsKey(boardName))
        {
            throw new KanbanException("The user is not a member of this board");
        }
        board.AddTask(user, dueDate, title, description, creationTime);
	}
	public void UpdateTaskDueDate(string email, string boardName, int taskId, int columnOrdinal, DateTime dueDate)
	{
        if (columnOrdinal > 2)
        {
            throw new KanbanException("no such column");
        }
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
		{
            throw new KanbanException("User is not logged in");
        }
        Board board= boards[boardName];
		if (board == null)
		{
			throw new KanbanException("This board name does not exists");
		}
        if (!user.Boards.ContainsKey(boardName))
        {
            throw new KanbanException("The user is not a member of this board");
        }
        if (columnOrdinal == 2)
        {
            throw new KanbanException("Tasks that are 'Done' can not be changed");
        }
        else
            board.UpdateTaskDueDate(email, taskId, columnOrdinal, dueDate);
	}
    public void UpdateTaskTitle(string email, string boardName, int taskId, int columnOrdinal, string title)
    {
        if (columnOrdinal > 1 | columnOrdinal < 0)
        {
            throw new KanbanException("no such column");
        }
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("User is not logged in");
        }
        Board board = boards[boardName];
        if (board == null)
        {
            throw new KanbanException("This board name does not exists");
        }
        if (!user.Boards.ContainsKey(boardName))
        {
            throw new KanbanException("The user is not a member of this board");
        }
        else
            board.UpdateTaskTitle(email, taskId, columnOrdinal, title);
    }
	public void UpdateTaskDescription(string email, string boardName, int taskId, int columnOrdinal, string description)
	{
        if (columnOrdinal > 2)
        {
            throw new KanbanException("no such column");
        }
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("User is not logged in");
        }
        Board board = boards[boardName];
        if (board == null)
        {
            throw new KanbanException("This board name does not exists");
        }
        if (!user.Boards.ContainsKey(boardName))
        {
            throw new KanbanException("The user is not a member of this board");
        }
        if (columnOrdinal == 2)
        {
            throw new KanbanException("Tasks that are 'Done' can not be changed");
        }
        else
            board.UpdateTaskDescription(email, taskId, columnOrdinal, description);
	}
	public void AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
	{
        if (columnOrdinal > 2)
        {
            throw new KanbanException("Tasks can not be changed to the giving column");
        }
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("User is not logged in");
        }
        Board board = boards[boardName];
        if (board == null)
        {
            throw new KanbanException("This board name does not exists");
        }
        if (!user.Boards.ContainsKey(boardName))
        {
            throw new KanbanException("The user is not a member of this board");
        }
        else
            board.AdvanceTask(email, columnOrdinal, taskId);
	}
	public void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
	{
        if (columnOrdinal > 2)
        {
            throw new KanbanException("no such column");
        }
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("User is not logged in");
        }
        Board board = boards[boardName];
        if (board == null)
        {
            throw new KanbanException("This board name does not exists");
        }
        if (!user.Boards.ContainsKey(boardName))
        {
            throw new KanbanException("The user is not a member of this board");
        }
        board.LimitColumn(columnOrdinal, limit);
    }
    public int GetColumnLimit(string email, string boardName, int columnOrdinal)
    {
        if (columnOrdinal > 2)
        {
            throw new KanbanException("no such column");
        }
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("User is not logged in");
        }
        Board board = boards[boardName];
        if (board == null)
        {
            throw new KanbanException("This board name does not exists");
        }
        if (!user.Boards.ContainsKey(boardName))
        {
            throw new KanbanException("The user is not a member of this board");
        }
        int lim = board.GetColumnLimit(columnOrdinal);
		return lim;
    }
    public string GetColumnName(string email, string boardName, int columnOrdinal)
    {
        if (columnOrdinal > 2)
        {
            throw new KanbanException("no such column");
        }
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("User is not logged in");
        }
        Board board = boards[boardName];
        if (board == null)
        {
            throw new KanbanException("This board name does not exists");
        }
        if (!user.Boards.ContainsKey(boardName))
        {
            throw new KanbanException("The user is not a member of this board");
        }
        string name = board.GetColumnName(columnOrdinal);
        return name;
    }
	public List<TaskToSend> GetColumn(string email, string boardName, int columnOrdinal)
	{
        if (columnOrdinal > 2)
        {
            throw new KanbanException("no such column");
        }
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("User is not logged in");
        }
        Board board = boards[boardName];
        if (board == null)
        {
            throw new KanbanException("This board name does not exists");
        }
        if (!user.Boards.ContainsKey(boardName))
        {
            throw new KanbanException("The user is not a member of this board");
        }
        List<TaskToSend> tasks = board.GetColumnToSend(columnOrdinal);
		return tasks;
    }
	public List<TaskToSend> GetInProgress(string email)
	{
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("User is not logged in");
        }
        List<Task> tasks = inProgressUser.GetList(email);
        List<TaskToSend> output = new List<TaskToSend>();
        foreach (Task task in tasks)
        {
            output.Add(new TaskToSend(task));
        }
        return output;
	}

    public void JoinBoard(string email, int boardID)
    {
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("user not logged in");
        }
        Board b = boards[idToNameBoards[boardID]];
        user.AddUserToBoard(b);
    }

    public void LeaveBoard(string email, int boardID)
    {
        User user = uf.GetUser(email);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("user not logged in");
        }
        Board b = boards[idToNameBoards[boardID]];
        if (b == null)
        {
            throw new KanbanException("this board does not exists");
        }
        if (user.Email.Equals(b.OwnerEmail))
        {
            throw new KanbanException("owner cannot leave his own bord");
        }
        if (! user.Boards.ContainsKey(b.Name))
        {
            throw new KanbanException("this user is not a member of this board");
        }
        int[] columns = {0, 1};
        foreach (var column in columns)
        {
            foreach (Task t in b.GetColumn(column))
            {
                if (t.EmailAssignee != null)
                {
                    if (t.EmailAssignee.Equals(email))
                    {
                        t.EmailAssignee = null;
                    }
                }
            }
        }
        user.DeleteUserFromBoard(b);
    }

    public string GetBoardName(int boardID)
    {
        return idToNameBoards[boardID];
    }
    public void TransferOwnership(string currentOwnerEmail, string newOwnewemail, string boardName)
    {
        User owner = uf.GetUser(currentOwnerEmail);
        User newOwner = uf.GetUser(newOwnewemail);
        if (!owner.IsLoggedIn())
        {
            throw new KanbanException("user not logged in");
        }
        Board b = boards[boardName];
        if (b == null)
        {
            throw new KanbanException("This board name does not exists");
        }
        if (!owner.Boards.ContainsKey(boardName) || !newOwner.Boards.ContainsKey(boardName))
        {
            throw new KanbanException("The user is not a member of this board");
        }
        b.TransferOwnership(newOwner);
    }

    public void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
    {
        User user = uf.GetUser(email);
        User newAssignee = uf.GetUser(emailAssignee);
        if (!user.IsLoggedIn())
        {
            throw new KanbanException("User is not logged in");
        }
        Board b = boards[boardName];
        if (b == null)
        {
            throw new KanbanException("This board name does not exists");
        }
        if (!user.Boards.ContainsKey(boardName) || !newAssignee.Boards.ContainsKey(boardName))
        {
            throw new KanbanException("The user is not a member of this board");
        }
        b.AssignTask(email, columnOrdinal, taskID, emailAssignee);
    }

    public void LoadData()
    {
        foreach (User u in uf.Users.Values)
        {
            foreach (Board b in u.Boards.Values)
            {
                if (!boards.ContainsKey(b.Name))
                {
                    boards.Add(b.Name, b);
                    idToNameBoards.Add(b.BoardID, b.Name);
                    copyInProgress(b.InProgressUser);
                }
            }
        }
    }

    public void copyInProgress(UserInProgressTasks u)
    {
        foreach(string email in u.UserTasks.Keys)
        {
            inProgressUser.addAllTasks(email, u.UserTasks[email]);
        }
    }

    public void DeleteData()
    {
        BoardController boardController = new BoardController();
        boardController.DeleteAllBoards();
        boardController.DeleteAllTasks();
    }

}
