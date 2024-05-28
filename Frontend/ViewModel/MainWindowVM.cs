using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    public class MainWindowVM : Notifiable
    {
        private string _email;
        private string _password;
        private string _errorMessage;
        private BackendController _backendController;

        public string Email
        {
            private get { return _email; }
            set { _email = value; }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }

        public string Password
        {
            private get { return _password; }
            set { _password = value; }
        }


        /// <summary>
        /// Initializes the VM
        /// </summary>
        public MainWindowVM()
        {
            _backendController = BackendController.Create();
            _email = "";
            _password = "";
            _errorMessage = "";
        }

        /// <summary>
        /// Initializes the VM with given BackendController
        /// </summary>
        /// <param name="backendController"></param>
        public MainWindowVM(BackendController backendController)
        {
            _backendController = backendController;
            _email = "";
            _password = "";
            _errorMessage = "";
        }

        /// <summary>
        /// Function logs in the user, if the user with the fields entered exists.
        /// </summary>
        /// <returns>the logged in user if the login was succsessful, otherwise shows the error occured to the user.</returns>
        public MUser Login()
        {
            MUser user = null;
            try
            {
                user = _backendController.Login(_email, _password);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return user;
        }
        /// <summary>
        /// Registers a new user in the system
        /// </summary>
        /// <returns>the logged in user if the registration was successful, otherwise shows the error occured to the user.</returns>
        public MUser Register()
        {
            try
            {
                return _backendController.Register(_email, _password);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }

        }
    }
}
