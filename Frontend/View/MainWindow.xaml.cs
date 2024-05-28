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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVM _vm;

        /// <summary>
        /// Constructs new MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM();
            _vm = (MainWindowVM)DataContext;
        }

        /// <summary>
        /// Constructs new MainWindow with the given backend controller.
        /// </summary>
        /// <param name="backendController"></param>
        public MainWindow(BackendController backendController)
        {
            InitializeComponent();
            DataContext = new MainWindowVM(backendController);
            _vm = (MainWindowVM)DataContext;
        }

        /// <summary>
        /// Moves to UserWindow if no errors occured.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {

            MUser user = _vm.Register();
            if (user != null)
            {
                UserWindow userWindow = new UserWindow(user);
                userWindow.Show();
                this.Close();
            }

        }

        /// <summary>
        /// Moves to UserWindow if no errors occured.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MUser user = _vm.Login();
            if (user != null)
            {
                UserWindow userWindow = new UserWindow(user);
                userWindow.Show();
                this.Close();
            }

        }
    }
}
