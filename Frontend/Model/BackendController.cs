using IntroSE.ForumSystem.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BackendController
    {
        private WrapperFactory wrapperFactory;
        private BackendController()
        {
            this.wrapperFactory = new WrapperFactory();
            this.wrapperFactory.create();
        }

        /// <summary>
        /// Creates new BackendController and loads data from the Data Base.
        /// </summary>
        /// <returns>the BackendController created.</returns>
        public static BackendController Create()
        {
            BackendController backendController = new BackendController();
            backendController.wrapperFactory.UserService.LoadData();
            backendController.wrapperFactory.BoardService.LoadData();
            return backendController;
        }

        /// <summary>
        /// Logs in the user with the given email and password if no errors occured.
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="password">password of the user</param>
        /// <returns>The logged in user if no errors occured, otherwise shows error to the user.</returns>
        /// <exception cref="Exception"></exception>
        public MUser Login(string email, string password)
        {
            Response res;
            string jsonRes = wrapperFactory.UserService.Login(email, password);
            res = JsonSerializer.Deserialize<Response>(jsonRes);

            if (res.ErrorMessage == null)
            {
                return new MUser(email, this);
            }
            else
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// Logs out the user with the given email if no errors occured.
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <exception cref="Exception"></exception>
        public void Logout(string email)
        {
            Response res;
            string jsonRes = wrapperFactory.UserService.Logout(email);
            res = JsonSerializer.Deserialize<Response>(jsonRes);

            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// Registers a user with the given email and password if no errors occured.
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="password">password of the user</param>
        /// <returns>the new user logged in, if no errors occured, otherwise shows the error to the user.</returns>
        /// <exception cref="Exception"></exception>
        public MUser Register(string email, string password)
        {
            Response res;
            string jsonRes = wrapperFactory.UserService.Register(email, password);
            res = JsonSerializer.Deserialize<Response>(jsonRes);

            if (res.ErrorMessage == null)
            {
                return new MUser(email, this);

            }
            else
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// Gets board with given boardId if no errors occured
        /// </summary>
        /// <param name="boardId">ID of the board</param>
        /// <param name="user">the user holding the board</param>
        /// <returns>the board with the given boardId if no errors occured.</returns>
        /// <exception cref="Exception"></exception>
        private MBoard GetMBoard(int boardId, MUser user)
        {
            Response res;
            string jsonRes = wrapperFactory.BoardService.GetBoardName(boardId);
            res = JsonSerializer.Deserialize<Response>(jsonRes);

            if (res.ErrorMessage == null)
            {
                return new MBoard(boardId, (string)ToObject<string>((JsonElement)res.ReturnValue), user, this);
            }
            else
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// Returnes all the boards of the given user if no errors occured.
        /// </summary>
        /// <param name="user">a logged in user</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<MBoard> GetAllBoards(MUser user)
        {
            Response listidBoard;
            string jsonRes = wrapperFactory.UserService.GetUserBoards(user.Email);
            listidBoard = JsonSerializer.Deserialize<Response>(jsonRes);
            List<MBoard> boards = new List<MBoard>();

            if (listidBoard.ErrorMessage == null)
            {
                List<int> idList = (List<int>)ToObject<List<int>>((JsonElement)listidBoard.ReturnValue);
                foreach (int id in idList)
                {
                    boards.Add(GetMBoard(id, user));
                }

                return boards;
            }
            else
            {
                throw new Exception(listidBoard.ErrorMessage);
            }
        }

        /// <summary>
        /// Gets all the tasks from a given column with a given board name and a given email, if no errors occured. 
        /// </summary>
        /// <param name="columnOrdinal">the number of the representing column</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="email">email of the user</param>
        /// <returns>a List of all the tasks, if no errors occured.</returns>
        /// <exception cref="Exception"></exception>
        public List<MTask> GetColumnTasks(int columnOrdinal, string boardName, string email)
        {
            Response res;
            string jsonRes = wrapperFactory.BoardService.GetColumn(email, boardName, columnOrdinal);
            res = JsonSerializer.Deserialize<Response>(jsonRes);

            if (res.ErrorMessage == null)
            {
                 return ConvertToMTask((List<TaskToSend>)ToObject<List<TaskToSend>>((JsonElement)res.ReturnValue));
            }
            else
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// Converts a List of type TaskToSend to a List of MTask, if no errors occured.
        /// </summary>
        /// <param name="tasks">List of TaskToSend</param>
        /// <returns>A List of MTask, if no errors occured.</returns>
        private List<MTask> ConvertToMTask(List<TaskToSend> tasks)
        {
            List<MTask> convertedTasks = new List<MTask> ();

            foreach(TaskToSend task in tasks)
            {
                convertedTasks.Add(new MTask(task));
            }

            return convertedTasks;
        }

        /// <summary>
        /// Deserializes JsonElement to dynamic type T.
        /// </summary>
        /// <typeparam name="T">dynamic type</typeparam>
        /// <param name="element">JsonElement to convert</param>
        /// <returns>The JsonElement converted to type T.</returns>
        private static T ToObject<T>(JsonElement element)
        {
            var json = element.GetRawText();
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
