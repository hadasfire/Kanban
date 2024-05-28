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
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        private BoardVM _vm;

        /// <summary>
        /// Constructs new BoardWindow.
        /// </summary>
        /// <param name="board">The baord we wish to open.</param>
        public BoardWindow(MBoard board)
        {
            InitializeComponent();
            DataContext = new BoardVM(board);
            _vm = (BoardVM)DataContext;
        }

        /// <summary>
        /// Moves to UserWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            UserWindow userWindow = new UserWindow(_vm.Board.User);
            userWindow.Show();
            this.Close();
        }
    }
}
