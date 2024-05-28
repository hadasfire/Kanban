using Frontend.Model;
using Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private UserVM _vm;

        /// <summary>
        /// Constructs a new UserWindow.
        /// </summary>
        /// <param name="user">The logged user which we want to show his UserWindow.</param>
        public UserWindow(MUser user)
        {
            InitializeComponent();
            DataContext = new UserVM(user);
            _vm = (UserVM)DataContext;
        }

        /// <summary>
        /// Moves to the selected boards window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterBoard_Click(object sender, RoutedEventArgs e)
        {
            MBoard board = _vm.SelectedBoard;
            if (board != null)
            {
                BoardWindow boardWindow = new BoardWindow(board);
                boardWindow.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Moves to the MainWindow if no errors occured.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if(_vm.Logout())
            {
                MainWindow mainWindow = new MainWindow(_vm.User.Controller);
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
