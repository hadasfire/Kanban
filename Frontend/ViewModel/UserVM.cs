using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    public class UserVM : Notifiable
    {
        private BackendController _backendController;
        private MUser _user;
        private MBoard _selectedBoard;
        private bool _enableForward;

        public MUser User
        {
            get { return _user; }
        }

        /// <summary>
        /// Constructs new UserVM.
        /// </summary>
        /// <param name="user">The user for which we construct the UserVM.</param>
        public UserVM(MUser user)
        {
            this._backendController = user.Controller;
            this._user = user;
            this._selectedBoard = null;
        }

        public MBoard SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                this._selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }

        }

        public bool EnableForward
        {
            get
            {
                return _enableForward;
            }
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <returns>true if logged out successfully, false otherwise.</returns>
        public bool Logout()
        {
            MUser user = null;
            try
            {
                _backendController.Logout(_user.Email);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
