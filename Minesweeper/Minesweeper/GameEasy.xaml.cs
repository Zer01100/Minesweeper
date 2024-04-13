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


            // Przypisz kolekcję obiektów CellData do ItemsSource gameBoardArray
            gameBoardArray.ItemsSource = gameBoard.Board;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Znajdź nadrzędny Grid dla przycisku
            var grid = FindParent<Grid>((Button)sender);

            // Znajdź pierwszy przycisk w kontenerze
            var button = (Button)VisualTreeHelper.GetChild(grid, 0);
            button.Visibility = Visibility.Collapsed; // Schowaj przycisk

            // Znajdź drugi element (TextBlock) w kontenerze
            var textBlock = (TextBlock)VisualTreeHelper.GetChild(grid, 1);
            textBlock.Visibility = Visibility.Visible; // Pokaż TextBlock
        }

        private DependencyObject FindParent<TDependencyObject>(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && !(parent is DependencyObject))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as DependencyObject;
        }
    }
}
