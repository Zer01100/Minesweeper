using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Minesweeper
{
    public partial class GameWindow : Window
    {

        private DispatcherTimer timer;
        private int seconds;
        private int rows;
        private int columns;

        public GameWindow(int rows,int columns)
        {
            this.rows = rows;
            this.columns = columns;
            InitializeComponent();
            InitializeTimer();
            GameBoard gameBoard = new GameBoard(8, 8, 10);

            CreateGridBoard();
        }

        public void CreateGridBoard()
        {

            //Definiowanie rozmiaru planszy
            for (int i = 0; i < rows; i++)
            {
                GridBoard.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            for (int j = 0; j < columns; j++)
            {
                GridBoard.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }

            //Dodawanie przycisków do planszy
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var button = new Button
                    {
                        Width = 30,
                        Height = 30,
                        Margin = new Thickness(1),
                        Tag = new Tuple<int, int, int>(i, j, 0)
                    };

                    //button.Click += ButtonClick;
                    //button.MouseRightButtonUp += ButtonFlag;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    GridBoard.Children.Add(button);
                }
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
            GameWindow GameWindow = new GameWindow(8,8);
            GameWindow.Left = this.Left;
            GameWindow.Top = this.Top;
            GameWindow.Show();
            this.Close();
        }
    }
}
