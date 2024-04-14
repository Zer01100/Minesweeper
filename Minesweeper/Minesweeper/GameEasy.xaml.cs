using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Minesweeper
{
    public partial class GameEasy : Window
    {

        private DispatcherTimer timer;
        private int seconds;
        public GameEasy()
        {
            InitializeComponent();
            InitializeTimer();
            GameBoard gameBoard = new GameBoard(8, 8, 10);

            gameBoardArray.ItemsSource = gameBoard.Board;
        }

        private void ButtonUncover(object sender, RoutedEventArgs e)
        {
            var grid = FindParent<Grid>((Button)sender);

            var button = (Button)VisualTreeHelper.GetChild(grid, 0);
            button.Visibility = Visibility.Collapsed;

            var textBlock = (TextBlock)VisualTreeHelper.GetChild(grid, 1);
            textBlock.Visibility = Visibility.Visible;

            if(textBlock.Text == "0")
            {
                UncoverZeroes(grid);
            }
            else if(textBlock.Text == "10")
            {
                //endGame();
            }
        }

        private void UncoverZeroes(Grid grid)
        {
            var row = Grid.GetRow(grid);
            var column = Grid.GetColumn(grid);


            for (int i = Math.Max(0, row - 1); i <= Math.Min(7, row + 1); i++)
            {
                for (int j = Math.Max(0, column - 1); j <= Math.Min(7, column + 1); j++)
                {
                    var neighboringGrid = gameBoardArray.ItemContainerGenerator.ContainerFromIndex(i * 8 + j) as Grid;
                    if (neighboringGrid != null)
                    {
                        var neighboringButton = (Button)VisualTreeHelper.GetChild(neighboringGrid, 0);
                        var neighboringTextBlock = (TextBlock)VisualTreeHelper.GetChild(neighboringGrid, 1);

                        if (neighboringButton.Visibility == Visibility.Visible && neighboringTextBlock.Text == "0")
                        {
                            neighboringButton.Visibility = Visibility.Collapsed;
                            neighboringTextBlock.Visibility = Visibility.Visible;
                            UncoverZeroes(neighboringGrid);
                        }
                    }
                }
            }
        }

        private Grid FindParent<TDependencyObject>(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && !(parent is DependencyObject))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as Grid;
        }

        private void ButtonFlagPlacement(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Button;

                if ((button.Content.ToString() == "0") && (int.Parse(FlagCounter.Text) > 0))
                {
                    button.Content = "1";
                    FlagCounter.Text = (int.Parse(FlagCounter.Text) - 1).ToString("D3");
                }
                else if ((button.Content.ToString()) == "1")
                {
                    button.Content = "0";
                    FlagCounter.Text = (int.Parse(FlagCounter.Text) + 1).ToString("D3");
                }
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
            if (seconds < 999)
            {
                seconds++;
                TimerTextBlock.Text = seconds.ToString("D3");
            }
            else
            {
                timer.Stop();
            }
        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            MainWindow MainMenu = new MainWindow();
            MainMenu.Show();
            this.Close();
        }

        private void GameRestart(object sender, MouseButtonEventArgs e)
        {
            GameEasy GameWindow = new GameEasy();
            GameWindow.Left = this.Left;
            GameWindow.Top = this.Top;
            GameWindow.Show();
            this.Close();
        }
    }
}
