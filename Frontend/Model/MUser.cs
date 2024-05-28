using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class MUser
    {
        public string _email;
        private BackendController _controller;
        public ObservableCollection<MBoard> Boards { get; set; }

        public BackendController Controller
        {
            get { return _controller; }
        }

        /// <summary>
        /// Constructs new MUser with given parameters.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="controller">User's password</param>
        public MUser(string email, BackendController controller)
        {
            this._email = email;
            this._controller = controller;
            Boards = new ObservableCollection<MBoard>(_controller.GetAllBoards(this));
        }

        public string Email
        {
            get { return _email; }
        }
    }
}
