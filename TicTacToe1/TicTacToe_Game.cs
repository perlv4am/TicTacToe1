using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TicTacToe1
{
    public class TicTacToe_Game
    {
        //Class that includes all information and methods about the game

        //initilize variables
        public int DimensionOfTheField;  //Dimension of the grid
        int[,] _gameField;
        public bool GameEnded = false;

        //Constructor for the game
        public TicTacToe_Game(int dimension)
        {
            DimensionOfTheField = dimension;
            _gameField = new int[,] {{0,0,2},{0,0,0},{0,0,0},{0,0,0}};
        }

        //Human has crosses
        public int AddCross(int[] Coordinates, TicTacToe_Game game)
        {
            int X = Coordinates[0];
            int Y = Coordinates[1];
            if (game._gameField[X, Y] == 0)
            {
                game._gameField[X, Y] = 1;
                return 1;
            }
            else
            {
                return 0;
            }
        }

        //Computer has rings
        public int AddRing(int[] Coordinates)
        {
            int X = Coordinates[0];
            int Y = Coordinates[1];

            if (_gameField[X, Y] == 0)
            {
                _gameField[X, Y] = 2;
                return 1;
            }
            else
            {
                return 0;
            }
        }

        //Read players input
        public void GetInput()
        {
            Console.WriteLine("Zadej X souradnici");
            int X_input = Convert.ToInt32(Console.ReadLine());

            if (X_input >= DimensionOfTheField)
                Console.WriteLine("Zahral jsi out");

            Console.WriteLine("Zadej Y souradnici");
            int Y_input = Convert.ToInt32(Console.ReadLine());

            if (X_input >= DimensionOfTheField)
                Console.WriteLine("Zahral jsi out");

            int[] Vstup = new int[] { X_input, Y_input };
            AddCross(Vstup, this);

            if ((WhoWins(_gameField) == 1) || (WhoWins(_gameField) == 2) || (WhoWins(_gameField) == 0))
            {
                GameEnded = true;
            }
        }

        //Print current game in console
        public void PrintGame()
        {
            Console.Clear();
            for (int ii = 0; ii < DimensionOfTheField; ii++)
            {
                Console.WriteLine(new string('-', 2 * DimensionOfTheField));
                for (int jj = 0; jj < DimensionOfTheField; jj++)
                {
                    Console.Write('|');
                    Console.Write(Convert.ToString(_gameField[ii, jj]));
                }
                Console.Write('|');
                Console.Write('\n');
            }
        }


        public void ComputerTurn()
        {
            int[] coordinates;
            int[,] score = new int[DimensionOfTheField, DimensionOfTheField];
            int[,] temporalField = new int[DimensionOfTheField, DimensionOfTheField];

            Parallel.For(0, DimensionOfTheField, ii =>
          {
              for (int jj = 0; jj < DimensionOfTheField; jj++)
              {
                  if (_gameField[ii, jj] == 0)
                  {
                      temporalField = (int[,])_gameField.Clone();
                      temporalField[ii, jj] = 2;

                        //Check if it is a winnig turn
                        if (WhoWins(temporalField) == 2)
                      {
                          score[ii, jj] = 10;
                      }

                        //Check if it is a loosing turn
                        if (WhoWins(temporalField) == 1)
                      {
                          score[ii, jj] = -10;
                      }

                        //Check if it is a draw game
                        if (WhoWins(temporalField) == 0)
                      {
                          score[ii, jj] = 0;
                      }

                      score[ii, jj] = WhereToPlay(1, temporalField);
                  }
                  else
                  {
                      score[ii, jj] = 6;
                  }
              }
          });            
            AddRing(FindMaximumCoordinates(score));
            if ((WhoWins(_gameField) == 1) || (WhoWins(_gameField) == 2) || (WhoWins(_gameField) == 0))
            {
                GameEnded = true;
            }
        }

        //Methods returns values of possible turns
        public int WhereToPlay(int player, int[,] gameField)
        {
            int[,] temporalField;
            int[] coordinates = new int[] { 0, 0 };
            int[,] score = new int[DimensionOfTheField, DimensionOfTheField];

            //Try all possibilities
            for (int ii = 0; ii < DimensionOfTheField; ii++)
            {
                for (int jj = 0; jj < DimensionOfTheField; jj++)
                {
                    if (gameField[ii, jj] == 0)
                    {
                        temporalField = (int[,])gameField.Clone();
                        temporalField[ii, jj] = player;

                        //Check if it is a winnig turn
                        if (WhoWins(temporalField) == 2)
                        {
                            return (10);
                        }

                        //Check if it is a loosing turn
                        if (WhoWins(temporalField) == 1)
                        {
                            return (-10);
                        }

                        //Check if it is a draw game
                        if (WhoWins(temporalField) == 0)
                        {
                            return (0);
                        }

                        //If it is not a winnig turn than evaluate other player's possibilities
                        if (player == 1)
                        {
                            score[ii, jj] = WhereToPlay(2, temporalField);
                        }
                        else
                        {
                            score[ii, jj] = WhereToPlay(1, temporalField);
                        }
                    }
                    else
                    {
                        score[ii, jj] = 6;
                    }
                }
            }
            return (EvaluatePosition(score, player));
        }



        //
        private int EvaluatePosition(int[,] score, int player)
        {

            if (player == 1)
            {
                return (FindMinimumValue(score));
            }
            else
            {
                return (FindMaximumValue(score));
            }
        }

        //Function looks for maximum number in 2D array
        private int FindMaximumValue(int[,] score)
        {
            int maximum = -1000;
            for (int ii = 0; ii < DimensionOfTheField; ii++)
            {
                for (int jj = 0; jj < DimensionOfTheField; jj++)
                {
                    if ((score[ii, jj] > maximum) && (score[ii, jj] != 6))
                    {
                        maximum = score[ii, jj];
                    }
                }
            }
            return (maximum);
        }

        //Function looks for coordinates of the maximum number in 2D array
        private int[] FindMaximumCoordinates(int[,] score)
        {
            int[] coordinates = new int[2];
            int maximum = -1000;
            for (int ii = 0; ii < DimensionOfTheField; ii++)
            {
                for (int jj = 0; jj < DimensionOfTheField; jj++)
                {
                    if ((score[ii, jj] > maximum)&&(score[ii, jj] != 6))
                    {
                        maximum = score[ii, jj];
                        coordinates[0] = ii;
                        coordinates[1] = jj;
                    }
                }
            }
            return (coordinates);
        }

        //Function looks for minimum number in 2D array
        private int FindMinimumValue(int[,] score)
        {
            int minimum = 1000;
            for (int ii = 0; ii < DimensionOfTheField; ii++)
            {
                for (int jj = 0; jj < DimensionOfTheField; jj++)
                {
                    if ((score[ii, jj] < minimum)&& (score[ii, jj] != 6))
                    {
                        minimum = score[ii, jj];
                    }
                }
            }
            return (minimum);
        }


        //Function looks for coordinates of the minimum number in 2D array
        private int[] FindMinimumCoordinates(int[,] score)
        {
            int[] coordinates = new int[2];
            int minimum = 1000;
            for (int ii = 0; ii < DimensionOfTheField; ii++)
            {
                for (int jj = 0; jj < DimensionOfTheField; jj++)
                {
                    if ((score[ii, jj] < minimum)&& (score[ii, jj] != 6))
                    {
                        minimum = score[ii, jj];
                        coordinates[0] = ii;
                        coordinates[1] = jj;
                    }
                }
            }
            return (coordinates);
        }

        public int WhoWins(int[,] gameField)
        {
            int length_Human = 0;
            int length_Computer = 0;

            //Check rows
            for (int ii = 0; ii < DimensionOfTheField; ii++)
            {
                for (int jj = 0; jj < DimensionOfTheField; jj++)
                {
                    if (gameField[ii, jj] == 1)
                    {
                        length_Human++;
                        length_Computer = 0;
                    }
                    else if (gameField[ii, jj] == 2)
                    {
                        length_Human = 0;
                        length_Computer++;
                    }
                    else
                    {
                        length_Computer = 0;
                        length_Human = 0;
                    }

                    if (length_Human == 3)
                        return 1;
                    if (length_Computer == 3)
                        return 2;
                }
                length_Computer = 0;
                length_Human = 0;
            }



            //Check columns
            for (int jj = 0; jj < DimensionOfTheField; jj++)
            {
                for (int ii = 0; ii < DimensionOfTheField; ii++)
                {

                    if (gameField[ii, jj] == 1)
                    {
                        length_Human++;
                        length_Computer = 0;
                    }
                    else if (gameField[ii, jj] == 2)
                    {
                        length_Human = 0;
                        length_Computer++;
                    }
                    else
                    {
                        length_Computer = 0;
                        length_Human = 0;
                    }

                    if (length_Human == 3)
                        return 1;
                    if (length_Computer == 3)
                        return 2;
                }
                length_Computer = 0;
                length_Human = 0;
            }



            //Check diagonal
            for (int ii = DimensionOfTheField - 1; ii >= 0; ii--)
            {
                for (int jj = 0; ii + jj < DimensionOfTheField; jj++)
                {
                    if (gameField[ii + jj, DimensionOfTheField - 1 - jj] == 1)
                    {
                        length_Human++;
                        length_Computer = 0;
                    }
                    else if (gameField[ii + jj, DimensionOfTheField - 1 - jj] == 2)
                    {
                        length_Human = 0;
                        length_Computer++;
                    }
                    else
                    {
                        length_Computer = 0;
                        length_Human = 0;
                    }

                    if (length_Human == 3)
                        return 1;
                    if (length_Computer == 3)
                        return 2;
                }
                length_Computer = 0;
                length_Human = 0;
            }



            for (int ii = 0; ii < DimensionOfTheField; ii++)
            {
                for (int jj = 0; DimensionOfTheField - 1 - jj - ii >= 0; jj++)
                {
                    if (gameField[jj, DimensionOfTheField - 1 - jj - ii] == 1)
                    {
                        length_Human++;
                        length_Computer = 0;
                    }
                    else if (gameField[jj, DimensionOfTheField - 1 - jj - ii] == 2)
                    {
                        length_Human = 0;
                        length_Computer++;
                    }
                    else
                    {
                        length_Computer = 0;
                        length_Human = 0;
                    }

                    if (length_Human == 3)
                        return 1;
                    if (length_Computer == 3)
                        return 2;
                }
                length_Computer = 0;
                length_Human = 0;
            }



            //Check anti-diagonal
            for (int ii = DimensionOfTheField - 1; ii > 0; ii--)
            {
                for (int jj = 0; ii + jj < DimensionOfTheField; jj++)
                {
                    if (gameField[ii + jj, jj] == 1)
                    {
                        length_Human++;
                        length_Computer = 0;
                    }
                    else if (gameField[ii + jj, jj] == 2)
                    {
                        length_Human = 0;
                        length_Computer++;
                    }
                    else
                    {
                        length_Computer = 0;
                        length_Human = 0;
                    }

                    if (length_Human == 3)
                        return 1;
                    if (length_Computer == 3)
                        return 2;
                }
                length_Computer = 0;
                length_Human = 0;
            }



            for (int ii = 0; ii < DimensionOfTheField; ii++)
            {
                for (int jj = 0; ii + jj < DimensionOfTheField; jj++)
                {
                    if (gameField[jj, ii + jj] == 1)
                    {
                        length_Human++;
                        length_Computer = 0;
                    }
                    else if (gameField[jj, ii + jj] == 2)
                    {
                        length_Human = 0;
                        length_Computer++;
                    }
                    else
                    {
                        length_Computer = 0;
                        length_Human = 0;
                    }

                    if (length_Human == 3)
                        return 1;
                    if (length_Computer == 3)
                        return 2;
                }
                length_Computer = 0;
                length_Human = 0;
            }


            //Check for draw game
            for (int ii = 0; ii < DimensionOfTheField; ii++)
            {
                for (int jj = 0; jj < DimensionOfTheField; jj++)
                {
                    if (gameField[ii, jj] == 0)
                    {
                        return (7);
                    }

                }
            }

            return (0);
        }

        public void Play()
        {
            int winner;
            while (true)
            {
                PrintGame();
                GetInput();
                PrintGame();
                if (GameEnded)
                {
                    winner = WhoWins(_gameField);
                    break;
                }
                ComputerTurn();
                PrintGame();
                if (GameEnded)
                {
                    winner = WhoWins(_gameField);
                    break;
                }
            }
            PrintGame();
            switch (winner)
            {
                case 1:
                    Console.WriteLine("Vyhral hrac" + winner.ToString());
                    break;
                case 2:
                    Console.WriteLine("Vyhral hrac" + winner.ToString());
                    break;
                case 0:
                    Console.WriteLine("Remiza");
                    break;
                default:
                    Console.WriteLine("Neco se pokazilo");
                    break;
            }
                    System.Threading.Thread.Sleep(3000);
        }


    }
}