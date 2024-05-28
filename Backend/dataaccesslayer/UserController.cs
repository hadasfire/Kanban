using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using IntroSE.Kanban.Backend.BuisnessLayer;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserController
    {
        private const string MessageTableName = "Users";
        private readonly string _connectionString;
        private readonly string _tableName;
        internal UserController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = MessageTableName;
        }
        public List<UserDTO> Select()
        {
            List<UserDTO> results = new List<UserDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = $"SELECT * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }
        public List<UserDTO> SelectAllUsers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();
            return result;
        }

        public List<BoardDTO> SelectBoardsByEmail(string email)
        {
            BoardController boardController = new BoardController();
            List<int> ids=boardController.SelectBoardsByEmail(email);
            List<BoardDTO> result = boardController.ConvertBoardIdtoBoardDTO(ids);
            return result;
        }

        public bool Insert(UserDTO user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName} (Email, Password) " +$"VALUES (@email,@password);";
                    SQLiteParameter emailParam = new SQLiteParameter("@email", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter("@password", user.Password);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch(Exception ex) 
                {
                    throw new KanbanDataException("Data Base exception");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        public bool Delete(UserDTO user)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where [Email]={user.Email}"
                };              
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }
        public bool DeleteAllUsers()
        {
            DeleteAllUsersInBoard();
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from Users"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        public bool DeleteAllUsersInBoard()
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from UsersInBoard"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        public bool AddMember(string email, int boardId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO UsersInBoard (UserEmail, BoardId) " + $"VALUES (@email,@boardId);";
                    SQLiteParameter emailParam = new SQLiteParameter("@email", email);
                    SQLiteParameter boardIdParam = new SQLiteParameter("@boardId", boardId);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(boardIdParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new KanbanDataException("Data Base exception");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        public bool DeleteMember(string email, int boardId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE from UsersInBoard WHERE BoardId = @boardId AND UserEmail = @userEmail";
                    command.Parameters.Add(new SQLiteParameter("@boardId", boardId));
                    command.Parameters.Add(new SQLiteParameter("@userEmail", email));
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new KanbanDataException("Data Base exception");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        public UserDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new UserDTO(reader.GetString(1),reader.GetString(0), SelectBoardsByEmail(reader.GetString(0)));
        }


    }

}
