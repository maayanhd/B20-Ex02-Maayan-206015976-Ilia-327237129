using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
     class UI
     {
          private UI()
          {

          }
          public static void StartGame()
          {
               // Getting board measurments from user 
               int[] boardMeasurements = new int[2];
               GetBoardMeasurementsFromUser(boardMeasurements);

               // generating memory cards based on the size of the board 
               List<int> memoryCards = new List<int>(boardMeasurements[0] * boardMeasurements[1]);
               GenerateMemArray(memoryCards);

               // Getting players' details from the user 
               string[] names = new string[2];
               Player.ePlayerType[] playersTypes = new Player.ePlayerType[2];
               GetPlayersDetailsFromUser(names, playersTypes);

               Game memoryGame = new Game(boardMeasurements, memoryCards, names, playersTypes);
          }
          public static string GetPlayerName()
          {
               string name = null;
               bool inputIsValid = false;
               Console.WriteLine("Please enter your name:");
               while (inputIsValid == false)
               {
                    name = Console.ReadLine();
                    inputIsValid = (name.Length > 0) ? true : false;
                    if (inputIsValid == false)
                    {
                         Console.WriteLine("Name cannot be empty,please try again");
                    }

               }

               return name;
          }
          public static void GetPlayersDetailsFromUser(string[] io_PlayersNames, Player.ePlayerType[] io_PlayersTypes)
          {
               bool inputIsValid = false;
               


               io_PlayersNames[0] = GetPlayerName();
               io_PlayersTypes[0] = Player.ePlayerType.Human;

               while (inputIsValid == false)
               {
                   
                    Console.WriteLine("Choose one of the game modes:");
                    Console.WriteLine("1.Play against human");
                    Console.WriteLine("2.Play against computer");
                    inputIsValid = int.TryParse(Console.ReadLine(), out int option);
                    if (inputIsValid == false)
                    {
                         Console.WriteLine("Error: You should enter an integer");
                    }
                    else if (option != 1 && option != 2)
                    {
                         Console.WriteLine("Error: Only the options 1,2 are legal");
                         inputIsValid = false;
                    }
                    else
                    {
                         io_PlayersNames[1] = (option == 1) ? GetPlayerName() : "Bot Harold";
                         io_PlayersTypes[1] = (option == 1) ? Player.ePlayerType.Human : Player.ePlayerType.Computer;
                    }
               }

          }

          public static int GetMeasurement()
          {
               int measurement=0;
               bool inputIsValid = false;
               while(inputIsValid == false)
               {
                    inputIsValid = int.TryParse(Console.ReadLine(),out measurement);
                    if(inputIsValid == false)
                    {
                         Console.WriteLine("The input must be a number");
                    }
                    else if(measurement <= 0)
                    {
                         Console.WriteLine("The value must be positive");
                    }
                    else if(measurement > 6)
                    {
                         Console.WriteLine("The maximum value is 6");
                    }
                    else
                    {
                         inputIsValid = true;
                    }
               }
               return measurement;
          }
          public static void GetBoardMeasurementsFromUser(int[] io_BoardMeasurementArr)
          {

               bool inputIsValid = false;

               while (inputIsValid == false)
               {
                    Console.WriteLine("Please enter the height of the board");
                    io_BoardMeasurementArr[0] = GetMeasurement();
                    Console.WriteLine("Please enter the width of the board");
                    io_BoardMeasurementArr[1] = GetMeasurement();

                    if((io_BoardMeasurementArr[0]*io_BoardMeasurementArr[1])%2!=0)
                    {
                         Console.WriteLine("The size of the board must be even");
                    }
                    else
                    {
                         inputIsValid = true;
                    }
               }
          }
          public static void GenerateMemArray(List<int> io_MemoryCards)
          {
               for(int i = 0; i < io_MemoryCards.Count; i+=2)
               {
                    io_MemoryCards[i] = io_MemoryCards[i+1] = 'A' + i;
               }
          }

          public enum eCurrentNumPlayer
          {
               Player1 = 1,
               Player2 = 2
          }

          public static void PrintBoard(Cell[,] i_Board)
          {
               int height = i_Board.GetLength(0);
               int width = i_Board.GetLength(1);

               
               PrintLettersLine(width);
               Console.Write(Environment.NewLine);
               PrintBorder(width);
               // Adding 1 to the height for printing th letters line 
               for (int i = 0; i < height; i++)
               {
                    String numberCell = String.Format("{0}  |", (i + 1).ToString());

                    Console.Write(numberCell);

                    // Adding 1 for the numbers color
                    for(int j= 0; j< width; j++) 
                    {
                         bool isFlipped = i_Board[i, j].IsFlipped;
                         char cellContent = ((isFlipped == true) ? (char)(i_Board[i, j].CellContent) : ' ' );
                         String cellString = String.Format(" {0} |", cellContent.ToString());

                         Console.Write(cellString);
                    }
                    Console.Write(Environment.NewLine);
                    PrintBorder(width);
               }
          }

          public static void PrintLettersLine(int i_BoardWidth)
          {
               String lettersLine = "   ";

               Console.Write(lettersLine);
               
               for (int i = 0; i < i_BoardWidth; i++)
               {
                    String letterCell = String.Format(" {0}  ", ((char)i + 'A').ToString());
                    Console.Write(letterCell);
               }
          }
          public static void PrintBorder(int i_BoardWidth)
          {
               StringBuilder borderEdge = new StringBuilder("   =");

               Console.Write(borderEdge);
               for(int i = 0; i< i_BoardWidth; i++)
               {
                    Console.Write("====");
               }
               Console.Write(Environment.NewLine);
          }
     }
}
