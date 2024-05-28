using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

public class User
{
    private UserDTO udto;
    private string password;
	private string email;
	private bool loggedIn;
	private Dictionary<string, Board> boards;


	public User(string password, string email)
	{
		this.udto = new UserDTO(password, email);
		this.password = password;
		this.email = email;
		this.loggedIn = true;
		this.boards = new Dictionary<string, Board>();
	}

    public User(UserDTO udto)
    {
        this.udto = udto;
        this.password = udto.Password;
        this.email = udto.Email; 
        this.loggedIn = false;
		List<BoardDTO> boardsDTO = udto.Boards;
        this.boards = new Dictionary<string, Board>();
        foreach (BoardDTO bd in boardsDTO)
		{
			Board b = new Board(bd);
			boards.Add(b.Name, b);
		}
    }


    public string Email { get => email; set => email = value; }
    public Dictionary<string, Board> Boards { get => boards; set => boards = value; }


    public void LogIn(string password) {
		if (!this.password.Equals(password)) {
			throw new KanbanException("password is wrong.");
		}
		if (this.loggedIn)
		{
			throw new KanbanException("user already logged in.");
		}
		this.loggedIn = true;
	}

	public bool IsLoggedIn() {
		return this.loggedIn;
	}

	public void LogOut()
	{
		if (!this.loggedIn)
		{
            throw new KanbanException("user already logged out.");
        }
        this.loggedIn = false;
    }

	public void AddUserToBoard(Board board)
	{
		this.boards.Add(board.Name, board);
		udto.addBoard(board.Bdto);
	}

    public void DeleteUserFromBoard(Board board)
    {
        this.boards.Remove(board.Name);
		udto.deleteBoard(board.Bdto);
    }

    public List<int> getUserBoards()
    {
        if (!this.loggedIn)
        {
            throw new KanbanException("User isn't log in.");
        }
        List<Board> toSend = boards.Values.ToList();
		List<int> output=new List<int>();
		foreach (Board board in toSend)
		{
			output.Add(board.BoardID);
		}
		return output;
    }
}
