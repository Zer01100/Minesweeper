﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGameEasy_Click(object sender, RoutedEventArgs e)
        {
            GameWindow GameWindow = new GameWindow(9,9,10);
            GameWindow.Left = Left;
            GameWindow.Top = Top;
            GameWindow.Show();
            Close();
        }

        private void StartGameMedium_Click(object sender, RoutedEventArgs e)
        {
            GameWindow GameWindow = new GameWindow(16, 16, 40);
            GameWindow.Left = Left;
            GameWindow.Top = Top;
            GameWindow.Show();
            Close();
        }

        private void StartGameHard_Click(object sender, RoutedEventArgs e)
        {
            GameWindow GameWindow = new GameWindow(16, 30, 99);
            GameWindow.Left = Left;
            GameWindow.Top = Top;
            GameWindow.Show();
            Close();
        }
    }
}
