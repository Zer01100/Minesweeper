using System;
using System.Collections.Generic;
using System.Data;
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

namespace Minesweeper
{
    public partial class GameEasy : Window
    {
        public GameEasy()
        {
            InitializeComponent();
            GameBoard gameBoard = new GameBoard(8, 8, 10);


            List<CellData> cellDataList = new List<CellData>();
            foreach (var cell in gameBoard.Board)
            {
                cellDataList.Add(cell);
            }

            // Przypisz kolekcję obiektów CellData do ItemsSource gameBoardArray
            this.DataContext = cellDataList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var cellData = button.DataContext as CellData;

            if (cellData != null)
            {
                cellData.IsVisible = true;
            }
        }
    }
}
