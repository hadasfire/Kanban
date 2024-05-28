using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class WrapperFactory
    {
        private UserFacade userFacade;
        private UserService userService;
        private BoardFacade boardFacade;
        private BoardService boardService;
        private TaskService taskService;

        public WrapperFactory() { 
        }

        public UserService UserService { get { return userService; }}
        public BoardService BoardService { get { return boardService;} }
        public TaskService TaskService { get { return taskService;} }

        public WrapperClass create()
        {
            userFacade = new UserFacade();
            userService = new UserService(userFacade);
            boardFacade = new BoardFacade(userFacade);
            boardService = new BoardService(boardFacade);
            taskService = new TaskService(boardFacade);
            
            return new WrapperClass(userService, boardService, taskService);
        }
        public void delete()
        {
            userService.DeleteData();
            boardService.DeleteData();
        }
    }
}
