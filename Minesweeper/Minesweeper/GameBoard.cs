using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class GameBoard
    {
        public int[] Board; // Jednowymiarowa tablica komórek planszy

        public int SizeX { get; private set; } // Liczba kolumn planszy
        public int SizeY { get; private set; } // Liczba wierszy planszy

        public GameBoard(int sizeX, int sizeY, int numberOfMines)
        {
            SizeX = sizeX;
            SizeY = sizeY;

            // Tworzenie planszy
            Board = new int[sizeX * sizeY];

            // Inicjalizacja komórek planszy
            for (int i = 0; i < sizeX * sizeY; i++)
            {
                Board[i] = 0; // Domyślnie komórka ma wartość 0
            }

            Random rand = new Random();

            // Generowanie min
            for (int i = 0; i < numberOfMines; i++)
            {
                int index = rand.Next(sizeX * sizeY);

                // Sprawdzenie, czy na danej pozycji już jest mina
                // Jeśli tak, powtórz losowanie
                if (Board[index] == 10)
                {
                    i--;
                    continue;
                }
                Board[index] = 10;
            }

            // Liczenie min na polach sąsiednich
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    int index = i * sizeY + j;
                    if (Board[index] != 10) // Jeśli pole nie zawiera miny
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
                                    int neighborIndex = nx * sizeY + ny;
                                    if (Board[neighborIndex] == 10)
                                    {
                                        liczbaMin++;
                                    }
                                }
                            }
                        }

                        // Aktualizacja wartości pola
                        Board[index] = liczbaMin;
                    }
                }
            }
        }
    }
}
