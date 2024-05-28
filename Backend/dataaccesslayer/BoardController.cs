using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BuisnessLayer;



namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardController
    {
        private const string MessageTableName = "Boards";
        private readonly string _connectionString;
        private readonly string _tableName;
        internal BoardController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = MessageTableName;
        }
        public List<BoardDTO> Select()
        {
            List<BoardDTO> results = new List<BoardDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * from {_tableName}";
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
        public List<BoardDTO> SelectAllBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            return result;
        }

        public List<int> SelectBoardsByEmail(string email)
        {
            List<int> results = new List<int>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = $"SELECT {"BoardId"} FROM UsersInBoard WHERE [UserEmail] = @Email";

                using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (SQLiteDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            int boardId = dataReader.GetInt32(0);
                            results.Add(boardId);
                        }
                    }
                }
            }
            return results;
        }

        public List<BoardDTO> ConvertBoardIdtoBoardDTO(List<int> boardId)
        {
            List<BoardDTO> results = new List<BoardDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                for (int i = 0; i < boardId.Count(); i = i + 1)
                {
                    int id = boardId[i];
                    SQLiteCommand command = new SQLiteCommand(null, connection);
                    command.CommandText = $"SELECT * FROM {_tableName} WHERE BoardId = {id}";

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
            }
            return results;
        }



        public bool Insert(BoardDTO board)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName} (BoardId, Name, TaskId, BackLogMax, InProgressMax, DoneMax, OwnerEmail) VALUES (@idVal, @nameVal, @taskIdVal, @backLogMaxVal, @inProgressMaxVal, @doneMaxVal, @ownerEmailVal)";
                    SQLiteParameter idParam = new SQLiteParameter("@idVal", board.BoardId);
                    SQLiteParameter nameParam = new SQLiteParameter("@nameVal", board.Name);
                    SQLiteParameter taskIdParam = new SQLiteParameter("@taskIdVal", board.TaskId);
                    SQLiteParameter backLogMaxParam = new SQLiteParameter("@backLogMaxVal", board.BackLogMax);
                    SQLiteParameter inProgressMaxParam = new SQLiteParameter("@inProgressMaxVal", board.InProgressMax);
                    SQLiteParameter doneMaxParam = new SQLiteParameter("@doneMaxVal", board.DoneMax);
                    SQLiteParameter ownerEmailParam = new SQLiteParameter("@ownerEmailVal", board.OwnerEmail);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(taskIdParam);
                    command.Parameters.Add(backLogMaxParam);
                    command.Parameters.Add(inProgressMaxParam);
                    command.Parameters.Add(doneMaxParam);
                    command.Parameters.Add(ownerEmailParam);
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new KanbanDataException(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
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
                    SQLiteParameter boardIdParam = new SQLiteParameter("@password", boardId);
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

        public bool Delete(BoardDTO board)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from {_tableName} where [BoardId]={board.BoardId}"
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


        public bool Update(int boardId, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,

                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where BoardId={boardId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    //command.Parameters.Add(new SQLiteParameter("@boardId", boardId));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new KanbanDataException(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }



        public bool Update(int boardId, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,

                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where BoardId={boardId}"
                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new KanbanDataException(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        public string SelectEmailOwner(int boardId)
        {
            string result="";

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = $"SELECT {"EmailOwner"} FROM {"Boards"} WHERE [BoardId] = @boardId";

                using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@boardId", boardId);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result= reader.GetString(0);
                        }
                    }
                }
            }

            return result;
        }

        public bool DeleteAllBoards()
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from {_tableName}"
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

        public bool DeleteAllTasks()
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from Tasks"
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

        public BoardDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            int boardId = reader.GetInt32(0);
            TaskController taskController=new TaskController();
            return new BoardDTO(reader.GetString(1), taskController.SelectTasksByBoardId(boardId), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5), taskController.SelectInProgressUser(), boardId, reader.GetString(6));
        }
    }
}
