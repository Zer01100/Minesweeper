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
        private int mines;

        public GameWindow(int rows, int columns, int mines)
        {
            this.rows = rows;
            this.columns = columns;
            this.mines = mines;
            InitializeComponent();
            InitializeTimer();
            FlagCounter.Text = mines.ToString();
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
                        FontSize = 20,
                        Margin = new Thickness(1),
                        Tag = new Tuple<int, int, int>(i, j, 0)
                    };

                    button.Click += ButtonShow;
                    button.MouseRightButtonUp += ButtonFlag;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    GridBoard.Children.Add(button);
                }
            }
        }

        //Mechanizm gry

        private void ButtonShow(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var position = (Tuple<int, int, int>)button.Tag;
            int row = position.Item1;
            int column = position.Item2;

            var textBlock = new TextBlock
            {
                Width = 30,
                Height = 30,
                FontSize = 20,
                Margin = new Thickness(1),
                Tag = new Tuple<int, int>(i, j)
            }
            Grid.SetRow(textBlock, row);
            Grid.SetColumn(textBlock, column);
            GridBoard.Children.Add(textBlock);
            //textBlock.Text = "!";


        }

        private void ButtonFlag(object sender, RoutedEventArgs e)
        {
            int FlagNumber = int.Parse(FlagCounter.Text);
            var button = (Button)sender;
            var position = (Tuple<int, int, int>)button.Tag;
            int row = position.Item1;
            int column = position.Item2;
            int state = position.Item3;
                switch (state)
                {

                    case 0:
                        button.Content = "🚩";
                        button.Tag = new Tuple<int, int, int>(row, column, 1);
                        FlagCounter.Text = (FlagNumber - 1).ToString();
                        break;
                    case 1:
                        button.Content = "?";
                        button.Tag = new Tuple<int, int, int>(row, column, 2);
                        break;
                    case 2:
                        button.Content = "";
                        button.Tag = new Tuple<int, int, int>(row, column, 0);
                        FlagCounter.Text = (FlagNumber + 1).ToString();
                        break;
                    default:
                        break;
                
            }
        }



        //Funkcje licznika
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
                TimerTextBlock.Text = seconds.ToString();
            }
            else
            {
                timer.Stop();
            }
        }


        //Pozostałe funkcje

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            MainWindow MainMenu = new MainWindow();
            MainMenu.Show();
            Close();
        }

        private void GameRestart(object sender, MouseButtonEventArgs e)
        {
            GameWindow GameWindow = new GameWindow(rows, columns, mines);
            GameWindow.Left = Left;
            GameWindow.Top = Top;
            GameWindow.Show();
            Close();
        }
    }
}
