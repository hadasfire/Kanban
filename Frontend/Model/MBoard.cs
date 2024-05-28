using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class MBoard
    {
        private string _name;
        private int _id;
        private string _email;
        private MUser _user;
        private BackendController _controller;
        public ObservableCollection<MTask> BacklogTasks { get; set; }
        public ObservableCollection<MTask> InProgressTasks { get; set; }
        public ObservableCollection<MTask> DoneTasks { get; set; }

        public string Name
        {
            get { return _name; }
        }

        public int Id
        {
            get { return _id; }
        }

        public BackendController Controller
        {
            get { return _controller; }
        }

        public MUser User
        {
            get { return _user; }
        }

        /// <summary>
        /// Constructs a new board with the given parameters.
        /// </summary>
        /// <param name="id">board's ID</param>
        /// <param name="name">board's name</param>
        /// <param name="user">The logged in member of the board</param>
        /// <param name="controller">The backend controller</param>
        public MBoard(int id, string name, MUser user, BackendController controller)
        {
            _id = id;
            _name = name;
            _user = user;
            _controller = controller;
            BacklogTasks = new ObservableCollection<MTask>(_controller.GetColumnTasks(0, name, user.Email));
            InProgressTasks = new ObservableCollection<MTask>(_controller.GetColumnTasks(1, name, user.Email));
            DoneTasks = new ObservableCollection<MTask>(_controller.GetColumnTasks(2, name, user.Email));
        }
    }
}
