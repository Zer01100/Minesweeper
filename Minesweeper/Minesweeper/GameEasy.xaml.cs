using System;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Schema;

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
        private int winCounter = 0;
        bool isBoardGenerated = false;

        public GameWindow(int rows, int columns, int mines)
        {
            Height = 360 + 32 * rows;
            Width = 160 + 32 * columns;

            this.rows = rows;
            this.columns = columns;
            this.mines = mines;
            InitializeComponent();
            InitializeTimer();
            FlagCounter.Text = mines.ToString();
            buttons = new Button[rows, columns];
            StatusEmoji.Fill = new ImageBrush(new BitmapImage(new Uri("smile.png", UriKind.Relative)));
            CreateGridBoard();
        }


        private int[,] GameBoardGenerator(int avoidRow, int avoidColumn)
        {
            // Tworzenie planszy
            int[,] Board = new int[rows, columns];

            // Inicjalizacja komórek planszy
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Board[i, j] = 0; // Domyślnie komórka ma wartość 0
                }
            }

            Random rand = new Random();

            // Generowanie min
            for (int i = 0; i < mines; i++)
            {
                int x, y;

                do
                {
                    x = rand.Next(rows);
                    y = rand.Next(columns);
                } while (x == avoidRow && y == avoidColumn); // Sprawdź, czy współrzędne nie są równe podanym

                // Sprawdzenie, czy na danej pozycji już jest mina
                if (Board[x, y] == 10)
                {
                    i--;
                    continue;
                }

                Board[x, y] = 10;
            }

            // Liczenie min na polach sąsiednich
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
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
                                if (nx >= 0 && nx < rows && ny >= 0 && ny < columns && Board[nx, ny] == 10)
                                {
                                    liczbaMin++;
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
                    button.PreviewMouseLeftButtonDown += ButtonLeftDown;
                    button.PreviewMouseLeftButtonUp += ButtonLeftUp;
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

            if(isBoardGenerated == false)
            {
                var position = (Tuple<int, int, int>)button.Tag;
                int row = position.Item1;
                int column = position.Item2;
                int flag = position.Item3;
                isBoardGenerated = true;
                board = GameBoardGenerator(row, column);
            }
            
            BoardButtonUncover(button);
        }

        private void BoardButtonUncover(Button button)
        {
            var position = (Tuple<int, int, int>)button.Tag;
            int row = position.Item1;
            int column = position.Item2;
            int flag = position.Item3;
            GridBoard.Children.Remove(button);
            buttons[row, column] = null;

            if (int.Parse(TimerTextBlock.Text) == 0)
            {
                timer.Start();
            }

            if (flag != 0)
            {
                FlagCounter.Text = (int.Parse(FlagCounter.Text)+1).ToString();
            }

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
                    textBlock.Background= Brushes.Red;
                    GameLost();
                    break;
                default:
                    break;
            }

            if(board[row, column] != 10)
            {
                winCounter++;
                if(winCounter >= ((rows * columns) - mines))
                {
                    GameWon();
                }
            }

            Grid.SetRow(textBlock, row);
            Grid.SetColumn(textBlock, column);
            GridBoard.Children.Add(textBlock);


        }

        private void ButtonFlag(object sender, MouseButtonEventArgs e)
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

        private void ButtonLeftDown(object sender, MouseButtonEventArgs e)
        {
            StatusEmoji.Fill = new ImageBrush(new BitmapImage(new Uri("fear.png", UriKind.Relative)));
        }

        private void ButtonLeftUp(object sender, MouseButtonEventArgs e)
        {
            StatusEmoji.Fill = new ImageBrush(new BitmapImage(new Uri("smile.png", UriKind.Relative)));
        }

        private void GameWon()
        {
            WinScreen.Visibility = Visibility.Visible;
            timer.Stop();
            StatusEmoji.Fill = new ImageBrush(new BitmapImage(new Uri("cool.png", UriKind.Relative)));
            BackButton.Foreground = Brushes.Red;
            LabelTime.Content = "Your time: " + TimerTextBlock.Text;
        }

        private void GameLost()
        {
            StatusEmoji.Fill = new ImageBrush(new BitmapImage(new Uri("dead.png", UriKind.Relative)));

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (board[i, j] == 10)
                    {
                        if (buttons[i, j] != null && buttons[i, j] is Button)
                        {
                            var textBlock = new TextBlock
                            {
                                Width = 30,
                                Height = 30,
                                FontSize = 20,
                                Margin = new Thickness(1),
                                Background = Brushes.White,
                                Tag = new Tuple<int, int>(i, j),
                                TextAlignment = TextAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Foreground = Brushes.Black,
                                FontWeight = FontWeights.Bold,
                                Text = "☼"
                            };

                            Grid.SetRow(textBlock, i);
                            Grid.SetColumn(textBlock, j);
                            GridBoard.Children.Add(textBlock);
                        }
                    }
                    if (buttons[i, j] != null)
                    {
                        buttons[i, j].IsEnabled = false;
                    }
                }
            }

            timer.Stop();

        }


        //Funkcje licznika
        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
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
            MainMenu.Left = Left;
            MainMenu.Top = Top;
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
