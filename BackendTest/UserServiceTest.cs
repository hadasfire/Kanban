using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.ForumSystem.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net.Layout;

namespace BackendTest
{

    class UserServiceTest
    {
        private readonly UserService userService;
        private readonly BoardService boardService;


        public UserServiceTest(WrapperClass w)
        {
            this.userService = w.userService;
            this.boardService = w.boardService;
        }

        public void runTests()
        {
            runTestSuccessfullRegister();
            runTestfailedRegisterByEmailNull();
            runTestfailedRegisterByInvalidPassword();
            runTestFailedlRegisterByUsedEmail();
            runTestSuccessfullLogOut();
            runTestFailedLogOutByNotLogInEmail();
            runTestSuccessfullLogIn();
            runTestFailedLogInByIncorrectPassword();
            runTestFailedLogInByNotRegisteredEmail();
            runTestSuccessfullGetUserBoards();
        }

        /// <summary>
        /// This function tests Requirement 2 and Requirement 7
        /// </summary>
        public void runTestSuccessfullRegister()
        {
            Response res1e = JsonSerializer.Deserialize<Response>(userService.Register("roni12@gmail.com", "R012356r"));
            if (res1e.ErrorOccured())
            {
                Console.WriteLine(res1e.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullRegister- succeeded.");
            }
            Response res1a = JsonSerializer.Deserialize<Response>(userService.Register("hadas12@gmail.com", "Aa012345"));
            if (res1a.ErrorOccured())
            {
                Console.WriteLine(res1a.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullRegister- succeeded.");
            }
            Response res1c = JsonSerializer.Deserialize<Response>(userService.Register("noga12@gmail.com", "A2345789a"));
            if (res1c.ErrorOccured())
            {
                Console.WriteLine(res1c.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullRegister- succeeded.");
            }
        }

        /// <summary>
        /// This function tests Requirement 3
        /// </summary>
        public void runTestfailedRegisterByEmailNull()
        {
            Response res1d = JsonSerializer.Deserialize<Response>(userService.Register(null, "Ab015234567"));
            if (res1d.ErrorOccured())
            {
                Console.WriteLine("runTestfailedRegisterByEmailNull- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestfailedRegisterByEmailNull- failed.");
            }
        }

        /// <summary>
        /// This function tests Requirement 2
        /// </summary>
        public void runTestfailedRegisterByInvalidPassword() 
        {
            Response res1b = JsonSerializer.Deserialize<Response>(userService.Register("noga1542@gmail.com", "0123"));
            if (res1b.ErrorOccured())
            {
                Console.WriteLine("unTestfailedRegisterByInvalidPassword- succeeded.");
            }
            else
            {
                Console.WriteLine("unTestfailedRegisterByInvalidPassword- failed.");
            }
        }

        /// <summary>
        /// This function tests Requirement 3
        /// </summary>
        public void runTestFailedlRegisterByUsedEmail()
        {
            Response res1c = JsonSerializer.Deserialize<Response>(userService.Register("hadas12@gmail.com", "Ab01234567"));
            if (res1c.ErrorOccured())
            {
                Console.WriteLine("runTestFailedlRegisterByUsedEmail- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedlRegisterByUsedEmail- failed.");
            }
        }


        /// <summary>
        /// This function tests Requirement 8
        /// </summary>
        public void runTestSuccessfullLogOut()
        {
            Response res2a = JsonSerializer.Deserialize<Response>(userService.Logout("hadas12@gmail.com"));
            if (res2a.ErrorOccured())
            {
                Console.WriteLine(res2a.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullLogOut- succeeded.");
            }
            Response res2c = JsonSerializer.Deserialize<Response>(userService.Logout("roni12@gmail.com"));
            if (res2c.ErrorOccured())
            {
                Console.WriteLine(res2c.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullLogOut- succeeded.");
            }
        }

        /// <summary>
        /// This function tests Requirement 8
        /// </summary>
        public void runTestFailedLogOutByNotLogInEmail()
        {
            Response res2b = JsonSerializer.Deserialize<Response>(userService.Logout("noga@gmail.com"));
            if (res2b.ErrorOccured())
            {
                Console.WriteLine("runTestFailedLogOutByNotLogInEmail- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedLogOutByNotLogInEmail- failed.");
            }
        }

        /// <summary>
        /// This function tests Requirement 8 and Requirement 1
        /// </summary>
        public void runTestSuccessfullLogIn()
        {
            Response res3a = JsonSerializer.Deserialize<Response>(userService.Login("hadas12@gmail.com", "Aa012345"));
            if (res3a.ErrorOccured())
            {
                Console.WriteLine(res3a.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullLogIn- succeeded.");
            }
        }

        /// <summary>
        /// This function tests Requirement 8 and Requirement 1
        /// </summary>
        public void runTestFailedLogInByIncorrectPassword()
        {
            Response res3b = JsonSerializer.Deserialize<Response>(userService.Login("roni12@gmail.com", "R012356rvc"));
            if (res3b.ErrorOccured())
            {
                Console.WriteLine("runTestFailedLogInByIncorrectPassword- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedLogInByIncorrectPassword- failed.");
            }
        }


        /// <summary>
        /// This function tests Requirement 8 and Requirement 1
        /// </summary>
        public void runTestFailedLogInByNotRegisteredEmail()
        {
            Response res3c = JsonSerializer.Deserialize<Response>(userService.Login("noga@gmail.com", "R01279rvc"));
            if (res3c.ErrorOccured())
            {
                Console.WriteLine("runTestFailedLogInByNotRegisteredEmail- succeeded.");
            }
            else
            {
                Console.WriteLine("runTestFailedLogInByNotRegisteredEmail- failed.");
            }
        }


        /// <summary>
        /// This function tests Requirement 5
        /// </summary>
        public void runTestSuccessfullGetUserBoards()
        {
            boardService.CreateBoard("noga12@gmail.com", "board1");
            boardService.CreateBoard("noga12@gmail.com", "board2");
            Response res = JsonSerializer.Deserialize<Response>(userService.GetUserBoards("noga12@gmail.com"));
            if (res.ErrorOccured())
            {
                Console.WriteLine(res.ErrorMessage);
            }
            else
            {
                Console.WriteLine("runTestSuccessfullGetUserBoards- succeeded.");
                Object o1 = res.ReturnValue;
                Console.WriteLine(o1.ToString());
            }
        }

    }
}
