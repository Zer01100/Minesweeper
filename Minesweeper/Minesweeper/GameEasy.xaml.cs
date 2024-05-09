using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Minesweeper
{
    public partial class GameWindow : Window
    {
        private int[,] board;
        Button[,] buttons;
        private DispatcherTimer timer;
        private int seconds;
        private int rows;
        private int columns;
        private int mines;

        public GameWindow(int rows, int columns, int mines)
        {
            Height = 360 + 30 * rows;
            Width = 160 + 30 * columns;
            this.rows = rows;
            this.columns = columns;
            this.mines = mines;
            InitializeComponent();
            InitializeTimer();
            FlagCounter.Text = mines.ToString();
            board = GameBoardGenerator(rows,columns,mines);
            buttons = new Button[rows, columns];

            CreateGridBoard();
        }


            private int[,] GameBoardGenerator(int sizeX, int sizeY, int numberOfMines)
            {

                // Tworzenie planszy
                int[,] Board = new int[sizeX, sizeY];

                // Inicjalizacja komórek planszy
                for (int i = 0; i < sizeX; i++)
                {
                    for (int j = 0; j < sizeY; j++)
                    {
                        Board[i, j] = 0; // Domyślnie komórka ma wartość 0
                    }
                }
                
                Random rand = new Random();

                // Generowanie min
                for (int i = 0; i < numberOfMines; i++)
                {
                    int x = rand.Next(sizeX);
                    int y = rand.Next(sizeY);

                    // Sprawdzenie, czy na danej pozycji już jest mina
                    // Jeśli tak, powtórz losowanie
                    if (Board[x, y] == 10)
                    {
                        i--;
                        continue;
                    }

                    Board[x, y] = 10;
                }

                // Liczenie min na polach sąsiednich
                for (int i = 0; i < sizeX; i++)
                {
                    for (int j = 0; j < sizeY; j++)
                    {
                        if (Board[i, j] != 10) // Jeśli pole nie zawiera miny
                        {
                            int liczbaMin = 0;

                            // Sprawdzanie sąsiednich pól
                            for (int dx = -1; dx <= 1; dx++)
                            {
                                for (int dy = -1; dy <= 1; dy++)
                                {
                                    int nx = i + dx;
                                    int ny = j + dy;

                                    // Sprawdzanie, czy współrzędne są w granicach planszy
                                    if (nx >= 0 && nx < sizeX && ny >= 0 && ny < sizeY)
                                    {
                                        if (Board[nx, ny] == 10)
                                        {
                                            liczbaMin++;
                                        }
                                    }
                                }
                            }

                            // Aktualizacja wartości pola
                            Board[i, j] = liczbaMin;
                        }
                    }
                }
            return Board;
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

                    button.Click += BoardButtonLeftClick;
                    button.MouseRightButtonUp += ButtonFlag;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    GridBoard.Children.Add(button);

                    buttons[i, j] = button;
                }
            }
        }

        //Mechanizm gry

        private void BoardButtonLeftClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            BoardButtonUncover(button);
        }

        private void BoardButtonUncover(Button button)
        {
            var position = (Tuple<int, int, int>)button.Tag;
            int row = position.Item1;
            int column = position.Item2;
            GridBoard.Children.Remove(button);
            buttons[row, column] = null;

            var textBlock = new TextBlock
            {
                Width = 30,
                Height = 30,
                FontSize = 20,
                Margin = new Thickness(1),
                Background = Brushes.White,
                Tag = new Tuple<int, int>(row, column),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            switch (board[row, column])
            {
                case 0:
                    if ((row + 1 < rows) && (buttons[row + 1, column] is Button)) { BoardButtonUncover(buttons[row + 1, column]); };
                    if ((row > 0) && (buttons[row - 1, column] is Button)) { BoardButtonUncover(buttons[row - 1, column]); };
                    if ((column + 1 < columns) && (buttons[row, column + 1] is Button)) { BoardButtonUncover(buttons[row, column + 1]); };
                    if ((column > 0) && (buttons[row, column - 1] is Button)) { BoardButtonUncover(buttons[row, column - 1]); };
                    break;
                case 1:
                    textBlock.Foreground = Brushes.Blue;
                    textBlock.Text = "1";
                    break;
                case 2:
                    textBlock.Foreground = Brushes.Green;
                    textBlock.Text = "2";
                    break;
                case 3:
                    textBlock.Foreground = Brushes.Red;
                    textBlock.Text = "3";
                    break;
                case 4:
                    textBlock.Foreground = Brushes.DarkBlue;
                    textBlock.Text = "4";
                    break;
                case 5:
                    textBlock.Foreground = Brushes.Brown;
                    textBlock.Text = "5";
                    break;
                case 6:
                    textBlock.Foreground = Brushes.Teal;
                    textBlock.Text = "6";
                    break;
                case 7:
                    textBlock.Foreground = Brushes.Black;
                    textBlock.Text = "7";
                    break;
                case 8:
                    textBlock.Foreground = Brushes.Gray;
                    textBlock.Text = "8";
                    break;
                case 10:
                    textBlock.Foreground = Brushes.Black;
                    textBlock.FontWeight = FontWeights.Bold;
                    textBlock.Text = "☼";
                    break;
                default:
                    break;
            }

            Grid.SetRow(textBlock, row);
            Grid.SetColumn(textBlock, column);
            GridBoard.Children.Add(textBlock);


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
