using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class WrapperClass
    {
        public UserService userService;
        public BoardService boardService;
        public TaskService taskService;


        public WrapperClass(UserService us, BoardService bs, TaskService ts) {
            userService = us;
            boardService = bs;
            taskService = ts;
        }
    }
}
