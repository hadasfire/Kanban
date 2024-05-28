using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    public class BoardVM : Notifiable
    {
        private BackendController _backendController;
        private MBoard _board;

        /// <summary>
        /// Constructs new BoardVM.
        /// </summary>
        /// <param name="board">The board to which we construct the BoardVM.</param>
        public BoardVM(MBoard board)
        {
            this._board = board;
            this._backendController = board.Controller;
        }

        public MBoard Board
        {
            get { return _board; }
        }
    }
}
