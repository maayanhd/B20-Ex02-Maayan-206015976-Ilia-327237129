using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace B20_Ex02
{
     class UI
     {
          private UI()
          {

          }
          internal static void Start()
          {
               {
                    // Getting players' details from the user 
                    string[] names = new string[2];
                    Player.ePlayerType[] playersTypes = new Player.ePlayerType[2];
                    GetPlayersDetailsFromUser(names, playersTypes);

                    // Getting board measurements from user 
                    int[] boardMeasurements = new int[2];
                    GetBoardMeasurementsFromUser(boardMeasurements);

                    // generating memory cards based on the size of the board 
                    List<int> memoryCards = new List<int>(boardMeasurements[0] * boardMeasurements[1]);
                    GenerateMemArray(memoryCards);

                    Game memoryGame = new Game(boardMeasurements, memoryCards, names, playersTypes);

                    MakeMoves(memoryGame);
               }
          }

          internal static void MakeMoves(Game io_MemoryGame)
          {
               Location? cardLocation1 = null,
                         cardLocation2 = null;
               Player currentPlayer = io_MemoryGame.Player1;


               while (io_MemoryGame.IsTheGameEnded() == false)
               {
                    // player's type is a human
                    if (currentPlayer.enumPlayerType == Player.ePlayerType.Human)
                    {
                         // First Card
                         cardLocation1 = GetCardLocationOrExit(io_MemoryGame, currentPlayer);
                         // Optional reuseability
                         Ex02.ConsoleUtils.Screen.Clear();
                         io_MemoryGame.FlipCard(cardLocation1); 
                         PrintBoard(io_MemoryGame.Board);

                         // Second Card
                         cardLocation2 = GetCardLocationOrExit(io_MemoryGame, currentPlayer);

                         CompleteMove(io_MemoryGame, currentPlayer, cardLocation1, cardLocation2);
                    }
                    else
                    {
                         currentPlayer.ComputerMove(io_MemoryGame.AvailableCards, io_MemoryGame.SeenCards);
                    }
                    // Updating current player
                    currentPlayer = (currentPlayer == io_MemoryGame.Player1) ? io_MemoryGame.Player2 : io_MemoryGame.Player1;
               }
          }

          internal static void CompleteMove(Game io_MemoryGame, Player io_CurrentPlayer, params Location?[] i_PairOfCards)
          {
               // Checking if there's a match and updating the status of card if necessary 
               bool isAMatch = io_MemoryGame.IsThereAMatch(io_CurrentPlayer, i_PairOfCards);
               
               // Flipping second card
               io_MemoryGame.FlipCard(i_PairOfCards[1]);
               Ex02.ConsoleUtils.Screen.Clear();
               PrintBoard(io_MemoryGame.Board);

               if (isAMatch == false)
               {
                    Thread.Sleep(2000);
                    // Flipping back cards
                    io_MemoryGame.FlipCard(i_PairOfCards[0]);
                    io_MemoryGame.FlipCard(i_PairOfCards[1]);
                    Ex02.ConsoleUtils.Screen.Clear();
                    PrintBoard(io_MemoryGame.Board);
               }

               io_MemoryGame.ManageAiStroage(isAMatch);
          }
          
          internal static Location? GetCardLocationOrExit(Game io_MemoryGame, Player CurrentPlayer)
          {
               Console.WriteLine(String.Format("{0}'s turn{1}",
                    CurrentPlayer.Name, 
                    Environment.NewLine));

               return GetHumanMove(io_MemoryGame);
          }
          
          internal static Location GetHumanMove(Game io_MemoryGame)
          {
               bool invalidInput = false;
              int column = 0, row = 0;
              
               do
               {
                    // Boolean contains the status of the last check of input
                    bool lengthIsValid   = false, 
                         isUpperCase     = false;

                    Console.WriteLine("Please enter a cell to expose a card: ");
                    string cellString = Console.ReadLine();

                    // Check Whether the 

                    

                    // Checking validity of the chosen cell
                    lengthIsValid = cellString.Length <= 2; 
                    isUpperCase   = lengthIsValid && char.IsUpper(cellString, 0);
                    column = (isUpperCase == true) ? (int)(cellString[0] - 'A') : -1; // Assigning -1 to column in case invalid
                    invalidInput  = (isUpperCase && char.IsDigit(cellString, 1) == false);

                    if(invalidInput == false)
                    {
                         Console.WriteLine("Invalid input location, Please try again.");
                    }
                    else
                    {
                         row = cellString[1] - 1;
                    }
               }
               while (invalidInput == true);

               Location location;
               location.m_Col = column;
               location.m_Row = row;
               return location;
          }

          internal static string GetPlayerName()
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
          internal static void GetPlayersDetailsFromUser(string[] io_PlayersNames, Player.ePlayerType[] io_PlayersTypes)
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
                         io_PlayersNames[1] = (option == 1) ? GetPlayerName() : "Computer";
                         io_PlayersTypes[1] = (option == 1) ? Player.ePlayerType.Human : Player.ePlayerType.Computer;
                    }
               }

          }

          internal static int GetMeasurement()
          {
               int measurement = 0;
               bool inputIsValid = false;
               while (inputIsValid == false)
               {
                    inputIsValid = int.TryParse(Console.ReadLine(), out measurement);
                    if (inputIsValid == false)
                    {
                         Console.WriteLine("The input must be a number");
                    }
                    else if (measurement <= 0)
                    {
                         Console.WriteLine("The value must be positive");
                    }
                    else if (measurement > 6)
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
          internal static void GetBoardMeasurementsFromUser(int[] io_BoardMeasurementArr)
          {

               bool inputIsValid = false;

               while (inputIsValid == false)
               {
                    Console.WriteLine("Please enter the height of the board");
                    io_BoardMeasurementArr[0] = GetMeasurement();
                    Console.WriteLine("Please enter the width of the board");
                    io_BoardMeasurementArr[1] = GetMeasurement();

                    if ((io_BoardMeasurementArr[0] * io_BoardMeasurementArr[1]) % 2 != 0)
                    {
                         Console.WriteLine("The size of the board must be even");
                    }
                    else
                    {
                         inputIsValid = true;
                    }
               }
          }
          internal static void GenerateMemArray(List<int> io_MemoryCards)
          {
               for (int i = 0; i < io_MemoryCards.Count; i += 2)
               {
                    io_MemoryCards[i] = io_MemoryCards[i + 1] = 'A' + i;
               }
          }

          internal static void PrintBoard(Cell[,] i_Board)
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
                    for (int j = 0; j < width; j++)
                    {
                         bool isFlipped = i_Board[i, j].IsFlipped;
                         char cellContent = ((isFlipped == true) ? (char)(i_Board[i, j].CellContent) : ' ');
                         String cellString = String.Format(" {0} |", cellContent.ToString());

                         Console.Write(cellString);
                    }
                    Console.Write(Environment.NewLine);
                    PrintBorder(width);
               }
          }

          internal static void PrintLettersLine(int i_BoardWidth)
          {
               String lettersLine = "   ";

               Console.Write(lettersLine);

               for (int i = 0; i < i_BoardWidth; i++)
               {
                    String letterCell = String.Format(" {0}  ", ((char)i + 'A').ToString());
                    Console.Write(letterCell);
               }
          }
          internal static void PrintBorder(int i_BoardWidth)
          {
               StringBuilder borderEdge = new StringBuilder("   =");

               Console.Write(borderEdge);
               for (int i = 0; i < i_BoardWidth; i++)
               {
                    Console.Write("====");
               }
               Console.Write(Environment.NewLine);
          }

          internal static void PlayGame()
          {
               bool isPlayingAgain = false;
               char answer = 'n';

               do
               {
                    bool isValidAnswer = false;
                    Start();

                    Console.WriteLine("Would you like to play again? y/n");
                    do
                    {
                         answer = char.Parse(Console.ReadLine());

                         isValidAnswer = (answer == 'y' || answer == 'n');

                         if(isValidAnswer)
                         {
                              isPlayingAgain = (answer == 'y');
                         }
                         else
                         {
                              Console.WriteLine("Invalid input");
                         }
                    }
                    while(isValidAnswer == false);
               }
               while (isPlayingAgain);
          }
     }
}
