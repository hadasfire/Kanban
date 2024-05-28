using IntroSE.ForumSystem.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTest
{
    class BoardServiceTest
    {
        private readonly BoardService boardService;
        private readonly UserService userService;
        private readonly TaskService taskService;

        public BoardServiceTest(WrapperClass w)
        {
            this.boardService=w.boardService;
            this.userService = w.userService;
            this.taskService = w.taskService;
        }

        public void createUsers()
        {
            userService.Register("hadas@gmail.com", "Aa0123457");
            userService.Register("roni@gmail.com", "R0123546r");
            userService.Register("noga@gmail.com", "A234589a");
        }

        public void runTests()
        {
            createUsers();
            runTestSuccessfullCreateBoard();
            runTestFailedCreateBoardBySameNameBoardToSameUser();
            runTestSuccessfullLimitColumn();
            runTestSucceessGetLimitColumn();
            runTestSucceessGetNameColumn();
            runTestSuccessfullJoinBoard();
            runTestFailedJoinBoard();
            runTestSuccessfullAddTask();
            runTestSuccessfullAddTaskByMember();
            runTestFailedAddTaskByNotMember();
            runTestFailedAddTaskByLimitColumn();
            runTestSuccessfullGetColumn();
            runTestSuccessfullDeleteBoard();
            runTestFailedAddTaskAfterDeleteBoard();
            runTestSuccessfullAdvanceTask();
            runTestFailedAdvanceTaskByNotAssignee();
            runTestFailedAdvanceTaskFromDone();
            runTestSuccessfullGetInProgress();
            runTestFailedGetInProgress();
            runTestSuccessfullLeaveBoard();
            runTestFailedLeaveBoardByNotLoggedIn();
            runTestFailedLeaveBoardByNotMember();
            runTestFailedLeaveBoardByOwner();
            runTestSuccessfullTransferOwnership();
            runTestFailedTransferOwnership();
            runTestSuccessfullGetBoardName();
            runTestFailedGetBoardNameDoesntExists();
        }

        /// <summary>
        /// This function tests Requirement 9
        /// </summary>
        public void runTestSuccessfullCreateBoard()
        {
            Response resp1a = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("hadas@gmail.com", "AssignmentHadas"));
            if (resp1a.ErrorOccured())
            {
                Console.WriteLine(resp1a.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullCreateBoard- succeeded.");
            }
            Response resp1c = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("roni@gmail.com", "Assignment1"));
            if (resp1c.ErrorOccured())
            {
                Console.WriteLine(resp1c.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullCreateBoard- succeeded.");
            }
            Response resp1d = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("roni@gmail.com", "Assignment2"));
            if (resp1d.ErrorOccured())
            {
                Console.WriteLine(resp1d.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullCreateBoard- succeeded.");
            }
            Response resp1e = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("noga@gmail.com", "Assignment3"));
            if (resp1e.ErrorOccured())
            {
                Console.WriteLine(resp1e.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullCreateBoard- succeeded.");
            }
        }

        /// <summary>
        /// This function tests Requirement 6
        /// </summary>
        public void runTestFailedCreateBoardBySameNameBoardToSameUser()
        {
            Response resp1b = JsonSerializer.Deserialize<Response>(boardService.CreateBoard("hadas@gmail.com", "AssignmentHadas"));
            if (resp1b.ErrorOccured())
            {
                Console.WriteLine("runTestFailedCreateBoardBySameNameBoardToSameUser- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedCreateBoardBySameNameBoardToSameUser- failed.");
            }
        }


        /// <summary>
        /// This function tests Requirement 16
        /// </summary>
        public void runTestSuccessfullLimitColumn()
        {
            Response resp2a = JsonSerializer.Deserialize<Response>(boardService.LimitColumn("hadas@gmail.com", "AssignmentHadas", 0, 3));
            if (resp2a.ErrorOccured())
            {
                Console.WriteLine(resp2a.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullLimitColumn- succeeded.");
            }
            Response resp2c = JsonSerializer.Deserialize<Response>(boardService.LimitColumn("roni@gmail.com", "Assignment1", 1, 3));
            if (resp2c.ErrorOccured())
            {
                Console.WriteLine(resp2c.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullLimitColumn- succeeded.");
            }
        }

        /// <summary>
        /// This function tests Requirement 16
        /// </summary>
        public void runTestSucceessGetLimitColumn()
       {
            Response resp3a = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit("hadas@gmail.com", "AssignmentHadas", 0));
            Object o = resp3a.ReturnValue;
            if (o.ToString().Equals("3"))
                Console.WriteLine("runTestSucceessGetLimitColumn- succeeded.");
            else
                Console.WriteLine("runTestSucceessGetLimitColumn- failed.");
        }


        /// <summary>
        /// This function tests Requirement 5
        /// </summary>
        public void runTestSucceessGetNameColumn()
        {
            Response resp3b = JsonSerializer.Deserialize<Response>(boardService.GetColumnName("roni@gmail.com", "Assignment1", 1));
            Object o1 = resp3b.ReturnValue;
            if (o1.ToString().Equals("in progress"))
                Console.WriteLine("runTestSucceessGetNameColumn- succeeded.");
            else
                Console.WriteLine("runTestSucceessGetNameColumn- failed.");
        }


        /// <summary>
        /// This function tests Requirement 12
        /// </summary>
        public void runTestSuccessfullJoinBoard()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.JoinBoard("roni@gmail.com",2));
            if (res.ErrorOccured())
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullJoinBoard- succeeded.");
            }
        }


        /// <summary>
        /// This function tests Requirement 12
        /// </summary>
        public void runTestFailedJoinBoard()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.JoinBoard("roni800@gmail.com", 2));
            if (res.ErrorOccured())
            {
                Console.WriteLine("runTestFailedJoinBoard- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedJoinBoard- failed.");
            }
        }


        /// <summary>
        /// This function tests Requirement 18
        /// </summary>
        public void runTestSuccessfullAddTask()
        {
            Response resp5a = JsonSerializer.Deserialize<Response>(boardService.AddTask("hadas@gmail.com", "AssignmentHadas", "Mission1", "your first mission", new DateTime(2023, 05, 20), new DateTime(2023, 04, 23)));
            if (resp5a.ErrorOccured())
            {
                Console.WriteLine(resp5a.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullAddTask- succeeded.");
                taskService.AssignTask("hadas@gmail.com", "AssignmentHadas", 0, 0, "roni@gmail.com"); // mission 1
            }
            
            Response resp5b = JsonSerializer.Deserialize<Response>(boardService.AddTask("hadas@gmail.com", "AssignmentHadas", "Mission2", "your second mission", new DateTime(2023, 05, 20), new DateTime(2023, 04, 23)));
            if (resp5b.ErrorOccured())
            {
                Console.WriteLine(resp5b.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullAddTask- succeeded.");
                taskService.AssignTask("hadas@gmail.com", "AssignmentHadas", 0, 1, "roni@gmail.com");// mission 2
            }
            
            Response resp5c = JsonSerializer.Deserialize<Response>(boardService.AddTask("noga@gmail.com", "Assignment3", "Mission4", "new mission", new DateTime(2023, 05, 20), new DateTime(2023, 04, 23)));
            if (resp5c.ErrorOccured())
            {
                Console.WriteLine(resp5c.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullAddTask- succeeded.");
            }
        }


        /// <summary>
        /// This function tests Requirement 18
        /// </summary>
        public void runTestSuccessfullAddTaskByMember()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.AddTask("roni@gmail.com", "AssignmentHadas", "mission3", "mission added by Roni", new DateTime(2023, 05, 20), new DateTime(2023, 04, 23)));
            if (res.ErrorOccured())
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullAddTaskByMember- succeeded.");
            }
        }

        /// <summary>
        /// This function tests Requirement 18
        /// </summary>
        public void runTestFailedAddTaskByNotMember()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.AddTask("noga@gmail.com", "AssignmentHadas", "mission4", "mission added by Noga", new DateTime(2023, 05, 20), new DateTime(2023, 04, 23)));
            if (res.ErrorOccured())
            {
                Console.WriteLine("runTestFailedAddTaskByNotMember- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedAddTaskByNotMember- failed.");
            }
        }

        /// <summary>
        /// This function tests Requirement 4
        /// </summary>
        public void runTestFailedAddTaskByLimitColumn()
        {
            Response resp5c = JsonSerializer.Deserialize<Response>(boardService.AddTask("hadas@gmail.com", "AssignmentHadas", "Mission3", "your third mission", new DateTime(2023, 05, 20), new DateTime(2023, 04, 23)));
            if (resp5c.ErrorOccured())
            {
                Console.WriteLine("runTestFailedAddTaskByLimitColumn- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedAddTaskByLimitColumn- failed.");
            }
        }

        /// <summary>
        /// This function tests Requirement 19
        /// </summary>
        public void runTestSuccessfullGetColumn()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.GetColumn("hadas@gmail.com", "AssignmentHadas", 0));
            if (res.ErrorOccured())
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullGetColumn- succeeded.");
                Object o1 = res.ReturnValue;
                Console.WriteLine(o1.ToString());
            }
        }


        /// <summary>
        /// This function tests Requirement 9
        /// </summary>
        public void runTestSuccessfullDeleteBoard()
        {
            Response resp5d = JsonSerializer.Deserialize<Response>(boardService.DeleteBoard("roni@gmail.com", "Assignment1"));
            if (resp5d.ErrorOccured())
            {
                Console.WriteLine(resp5d.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullDeleteBoard- succeeded.");
            }
        }


        /// <summary>
        /// This function tests Requirement 9
        /// </summary>
        public void runTestFailedAddTaskAfterDeleteBoard()
        {
            Response resp5a = JsonSerializer.Deserialize<Response>(boardService.AddTask("roni@gmail.com", "Assignment1", "Mission2", "your first mission", new DateTime(2023, 05, 20), new DateTime(2023, 04, 23)));
            if (resp5a.ErrorOccured())
            {
                Console.WriteLine("runTestFailedAddTaskAfterDeleteBoard- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedAddTaskAfterDeleteBoard- failed.");
            }
        }

        /// <summary>
        /// This function tests Requirement 19
        /// </summary>
        public void runTestSuccessfullAdvanceTask()
        {
            Response resp5a = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("roni@gmail.com", "AssignmentHadas", 0, 0));
            if (resp5a.ErrorOccured())
            {
                Console.WriteLine(resp5a.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullAdvanceTask- succeeded.");
            }
            Response resp5b = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("roni@gmail.com", "AssignmentHadas", 1, 0));
            if (resp5b.ErrorOccured())
            {
                Console.WriteLine(resp5b.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullAdvanceTask- succeeded.");
            }
            Response resp5c = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("roni@gmail.com", "AssignmentHadas", 0, 1));
            if (resp5c.ErrorOccured())
            {
                Console.WriteLine(resp5c.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullAdvanceTask- succeeded.");
            }
        }

        /// <summary>
        /// This function tests Requirement 19
        /// </summary>
        public void runTestFailedAdvanceTaskByNotAssignee()
        {
            Response resp5c = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("hadas@gmail.com", "AssignmentHadas", 1, 1));
            if (resp5c.ErrorOccured())
            {
                Console.WriteLine("runTestFailedAdvanceTaskByNotAssignee- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedAdvanceTaskByNotAssignee- failed.");
            }
        }

        /// <summary>
        /// This function tests Requirement 19
        /// </summary>
        public void runTestFailedAdvanceTaskFromDone()
        {
            Response resp5b = JsonSerializer.Deserialize<Response>(boardService.AdvanceTask("hadas@gmail.com", "AssignmentHadas", 2, 0));
            if (resp5b.ErrorOccured())
            {
                Console.WriteLine("runTestFailedAdvanceTaskFromDone- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedAdvanceTaskFromDone- failed.");
            }
        }


        /// <summary>
        /// This function tests Requirement 22
        /// </summary>
        public void runTestSuccessfullGetInProgress()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.InProgressTasks("roni@gmail.com"));
            if (res.ErrorOccured())
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullGetInProgress- succeeded.");
                Object o1 = res.ReturnValue;
                Console.WriteLine(o1.ToString());
            }  
        }


        /// <summary>
        /// This function tests Requirement 22
        /// </summary>
        public void runTestFailedGetInProgress()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.InProgressTasks("hadas800@gmail.com"));
            if (res.ErrorOccured())
            {
                Console.WriteLine("runTestFailedGetInProgress- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedGetInProgress- failed.");
            }
        }


        /// <summary>
        /// This function tests Requirement 12
        /// </summary>
        public void runTestSuccessfullLeaveBoard() 
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard("roni@gmail.com",2));
            if (res.ErrorOccured())
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullLeaveBoard- succeeded.");
            }
        }


        /// <summary>
        /// This function tests Requirement 12
        /// </summary>
        public void runTestFailedLeaveBoardByNotLoggedIn()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard("roni800@gmail.com", 2));
            if (res.ErrorOccured())
            {
                Console.WriteLine("runTestFailedLeaveBoardByNotLoggedIn- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedLeaveBoardByNotLoggedIn- failed.");
            }
        }


        /// <summary>
        /// This function tests Requirement 12
        /// </summary>
        public void runTestFailedLeaveBoardByNotMember()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard("roni@gmail.com", 2));
            if (res.ErrorOccured())
            {
                Console.WriteLine("runTestFailedLeaveBoardByNotMember- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedLeaveBoardByNotMember- failed.");
            }
        }


        /// <summary>
        /// This function tests Requirement 14
        /// </summary>
        public void runTestFailedLeaveBoardByOwner()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard("hadas@gmail.com", 2));
            if (res.ErrorOccured())
            {
                Console.WriteLine("runTestFailedLeaveBoardByOwner- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedLeaveBoardByOwner- failed.");
            }
        }


        /// <summary>
        /// This function tests Requirement 13
        /// </summary>
        public void runTestSuccessfullTransferOwnership()
        {
            boardService.JoinBoard("roni@gmail.com", 5);
            Response res = JsonSerializer.Deserialize<Response>(boardService.TransferOwnership("noga@gmail.com", "roni@gmail.com", "Assignment3"));
            if (res.ErrorOccured())
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullTransferOwnership- succeeded.");
            }
        }


        /// <summary>
        /// This function tests Requirement 13
        /// </summary>
        public void runTestFailedTransferOwnership()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.TransferOwnership("roni@gmail.com", "hadas@gmail.com", "Assignment2"));
            if (res.ErrorOccured())
            {
                Console.WriteLine("runTestFailedTransferOwnership- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedTransferOwnership- failed.");
            }
        }

        /// <summary>
        /// This function tests Requirement 5
        /// </summary>
        public void runTestSuccessfullGetBoardName()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(2));
            if (res.ErrorOccured())
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Object o1 = res.ReturnValue;
                if (o1.ToString().Equals("AssignmentHadas"))
                {
                    Console.WriteLine("runTestSuccessfullGetBoardName- succeeded.");
                }
                else
                {
                    Console.WriteLine("runTestSuccessfullGetBoardName- failed.");
                }
            }
        }

        /// <summary>
        /// This function tests Requirement 5
        /// </summary>
        public void runTestFailedGetBoardNameDoesntExists()
        {
            Response res = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(6));
            if (res.ErrorOccured())
            {
                Console.WriteLine("runTestFailedGetBoardNameDoesntExists- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedGetBoardNameDoesntExists- failed.");
            }
        }


    }
}
