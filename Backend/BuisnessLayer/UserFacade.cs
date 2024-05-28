using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;

public class UserFacade
{
	private Dictionary<string, User> users;
	public UserFacade()
	{
		this.users = new Dictionary<string, User>();
	}

    public Dictionary<string, User> Users { get => users; }


    public void Register(string email, string password)
	{
		if (email == null)
		{
            throw new KanbanException("email can't be null.");

        }
        if (this.users.ContainsKey(email))
		{
            throw new KanbanException("email already exists.");
        }
        if (password.Length>20 | password.Length<6) 
		{
			throw new KanbanException("invalid password. A valid password is in the length of 6 to 20 characters and must include at least one uppercase letter, one small character, and a number.");
		}
		int counterBigger = 0;
		int counterSmaller = 0;
		int countNumbers = 0;
		for (int i=0; i<password.Length; i++)
		{
			char temp = password[i];
			if (char.IsUpper(temp))
				counterBigger++;
			else if (char.IsLower(temp))
				counterSmaller++;
			else 
				countNumbers++;
		}
		if (counterBigger < 1 | counterSmaller < 1  | countNumbers < 1) {
            throw new KanbanException("invalid password. A valid password is in the length of 6 to 20 characters and must include at least one uppercase letter, one small character, and a number.");
        }
		User newOne = new User(password, email);
		this.users.Add(email, newOne); //wont happen if new user failed- with insert to data base
    }

	public string LogIn(string email, string password)
	{
		if (!this.users.ContainsKey(email))
		{
            throw new KanbanException("you need to register.");
        }
		this.users[email].LogIn(password);
		return email;
    }

	public void LogOut(string email)
	{
		if (!this.users.ContainsKey(email))
		{
            throw new KanbanException("there isn't user with this email, you can't log out.");
        }
        this.users[email].LogOut();
	}

	public User GetUser(string email)
	{
		if (!this.users.ContainsKey(email))
			throw new KanbanException("user doesn't exist");
		return this.users[email];
	}

	public User GetUser(UserDTO u)
	{
		if (!this.users.ContainsKey(u.Email))
		{
			this.users[u.Email] = new User(u);
		}
		return this.users[u.Email];
	}

    public List<int> getUserBoards(string email)
	{
		if (!this.users.ContainsKey(email))
			throw new KanbanException("user doesn't exist");
		return users[email].getUserBoards();
	}

    public void DeleteBoardFromAllUsers(Board b)
	{
		foreach(User user in users.Values)
		{
			if (user.Boards.ContainsKey(b.Name))
			{
				user.DeleteUserFromBoard(b);
			}
		}
	}

	public void LoadData()
	{
		List<UserDTO> userDtos = new UserController().SelectAllUsers();
		foreach (UserDTO userDTO in userDtos)
		{
			User u = GetUser(userDTO);
			users[u.Email] = u;
		}
	}

	public void DeleteData()
	{
		UserController userController = new UserController();
		userController.DeleteAllUsers();
	}

}
