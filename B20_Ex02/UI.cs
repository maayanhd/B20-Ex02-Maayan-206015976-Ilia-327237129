using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace B20_Ex02
{
     public class UI
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
                    GenerateMemoryCards(memoryCards, boardMeasurements[0] * boardMeasurements[1]);

                    Game memoryGame = new Game(boardMeasurements, memoryCards, names, playersTypes);

                    HandleMoves(memoryGame);
               }
          }

          internal static void HandleMoves(Game io_MemoryGame)
          {
               Player currentPlayer = io_MemoryGame.Player1;
               Cell[] chosenCards = new Cell[2];
               
               while (io_MemoryGame.IsTheGameEnded() == false)
               {
                    Ex02.ConsoleUtils.Screen.Clear();
                    PrintBoard(io_MemoryGame);
                    Console.WriteLine(string.Format(
                                        "{0}{1}'s turn{0}", 
                                        Environment.NewLine, 
                                        currentPlayer.Name));

                    // player's type is a human
                    if (currentPlayer.EPlayerType == Player.ePlayerType.Human)
                    {
                         // First Card
                         chosenCards[0] = GetHumanMove(io_MemoryGame);
                    }
                    else
                    {
                         chosenCards = currentPlayer.ComputerMove(io_MemoryGame);
                    }

                    io_MemoryGame.FlipCard(chosenCards[0]);
                    Ex02.ConsoleUtils.Screen.Clear();
                    PrintBoard(io_MemoryGame);
                    chosenCards[1] = (currentPlayer.EPlayerType == Player.ePlayerType.Human) ? GetHumanMove(io_MemoryGame) : chosenCards[1];

                    bool isAMatch = CompleteMove(io_MemoryGame, currentPlayer, chosenCards);

                    // Updating current player - swapping in case there isn't a match 
                    if(isAMatch == false)
                    {
                         currentPlayer = (currentPlayer == io_MemoryGame.Player1) ? io_MemoryGame.Player2 : io_MemoryGame.Player1;
                    }
                    else
                    {
                         Console.Write(Environment.NewLine);
                         Console.WriteLine("Well done, you earned another turn!");
                    }
               }

               DeclareWinner(io_MemoryGame.Player1, io_MemoryGame.Player2);
          }

          public static void DeclareWinner(params Player[] i_Players)
          {
               Console.WriteLine(string.Format(
                                   "{2}{0}'s Score: {1}{2}{3}'s Score: {4}",
                                   i_Players[0].Name,
                                   i_Players[0].Score.ToString(),
                                   Environment.NewLine,
                                   i_Players[1].Name,
                                   i_Players[1].Score.ToString()));

               if (i_Players[0].Score == i_Players[1].Score)
               {
                    Console.WriteLine("The game ended in a draw");
               }
               else
               {
                    string winnerName = (i_Players[0].Score > i_Players[1].Score) ? i_Players[0].Name : i_Players[1].Name;

                    Console.WriteLine(string.Format(
                                        "{0} won",
                                        winnerName));
               }
          }

          // Complete player's move and determines whether a pair has found 
          internal static bool CompleteMove(Game io_MemoryGame, Player io_CurrentPlayer, params Cell[] io_ChosenCards)
          {
               // Checking if there's a match and updating the status of card if necessary 
               bool isAMatch = io_MemoryGame.IsThereAMatch(io_CurrentPlayer, io_ChosenCards);

               // Flipping second card
               io_MemoryGame.FlipCard(io_ChosenCards[1]);
               Ex02.ConsoleUtils.Screen.Clear();
               PrintBoard(io_MemoryGame);

               if (isAMatch == false)
               {    // Screen suspension for 2 seconds
                    Thread.Sleep(2000);

                    // Flipping back cards
                    io_MemoryGame.FlipCard(io_ChosenCards[0]);
                    io_MemoryGame.FlipCard(io_ChosenCards[1]);
                    Ex02.ConsoleUtils.Screen.Clear();
               }

               // Logic of data structures 
               io_MemoryGame.UpdateAvailableCards(isAMatch, io_ChosenCards);
               io_MemoryGame.UpdateSeenCards(isAMatch, io_ChosenCards);

               return isAMatch;
          }

          internal static Cell GetHumanMove(Game i_MemoryGame)
          {
               bool inputIsValid = false;
               Console.WriteLine("Please enter a cell to expose a card: ");
               Cell cardChosen = null;

               while (inputIsValid == false)
               {
                    string cellString = Console.ReadLine();
                    Location cardLocation;

                    // Checking whether the player wants to exit the game
                    if (cellString.Equals("Q"))
                    {
                         Environment.Exit(0);
                    }

                    // Validation checks 
                    if (cellString.Length != 2 || char.IsUpper(cellString[0]) == false || char.IsDigit(cellString[1]) == false)
                    {
                         Console.WriteLine("The input must be a capital letter following by a digit, for example: A2,C3,E1");
                    }
                    else if (i_MemoryGame.IsLocationInRange(cardLocation = GetLocationFromStr(cellString)) == false)
                    {
                         Console.WriteLine("The location must be in range of the board");
                    }
                    else
                    {
                         if (i_MemoryGame.Board[cardLocation.Row, cardLocation.Col].IsFlipped)
                         {
                              Console.WriteLine("The card is already exposed");
                         }
                         else
                         {
                              inputIsValid = true;
                              cardChosen = i_MemoryGame.Board[cardLocation.Row, cardLocation.Col];
                         }
                    }
               }

               return cardChosen;
          }

          internal static Location GetLocationFromStr(string i_LocationStr)
          {
               return new Location(int.Parse(i_LocationStr[1].ToString()) - 1, i_LocationStr[0] - 'A');
          }

          internal static string GetPlayerName()
          {
               string name = null;
               bool inputIsValid = false;
               do
               {
                    Console.WriteLine("Please enter your name:");
                    name = Console.ReadLine();
                    inputIsValid = name.Length > 0;

                    if (inputIsValid == false)
                    {
                         Console.WriteLine("Name cannot be empty,please try again");
                         inputIsValid = false;
                    }
               }
               while (inputIsValid == false);

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

          internal static int GetMeasurementFromUser()
          {
               bool inputIsValid = false;
               int measurementInput = 0;

               while (inputIsValid == false)
               {
                    inputIsValid = int.TryParse(Console.ReadLine(), out measurementInput);
                    if (inputIsValid == false)
                    {
                         Console.WriteLine("The input must be a number, please try again");
                    }
                    else if (measurementInput < 4)
                    {
                         Console.WriteLine("The minimum value is 4, please try again");
                         inputIsValid = false;
                    }
                    else if (measurementInput > 6)
                    {
                         Console.WriteLine("The maximum value is 6, please try again");
                         inputIsValid = false;
                    }
                    else
                    {
                         inputIsValid = true;
                    }
               }

               return measurementInput;
          }

          internal static void GetBoardMeasurementsFromUser(int[] io_BoardMeasurementArr)
          {
               bool inputIsValid = false;

               do
               {
                    Console.WriteLine("Please enter the height of the board");
                    io_BoardMeasurementArr[0] = GetMeasurementFromUser();
                    Console.WriteLine("Please enter the width of the board");
                    io_BoardMeasurementArr[1] = GetMeasurementFromUser();

                    if ((io_BoardMeasurementArr[0] * io_BoardMeasurementArr[1]) % 2 != 0)
                    {
                         Console.WriteLine("The size of the board must be even");
                    }
                    else
                    {
                         inputIsValid = true;
                    }
               }
               while (inputIsValid == false);
          }

          internal static void GenerateMemoryCards(List<int> io_MemoryCards, int i_NumOfCards)
          {
               for (int i = 0; i < i_NumOfCards / 2; i++)
               {
                    io_MemoryCards.Add((int)'A' + i);
                    io_MemoryCards.Add((int)'A' + i);
               }
          }

          internal static void PrintBoard(Game i_MemoryCardGame)
          {
               int height = i_MemoryCardGame.Board.GetLength(0);
               int width = i_MemoryCardGame.Board.GetLength(1);

               PrintLettersLine(width);
               Console.Write(Environment.NewLine);
               PrintBorder(width);
               
               // Adding 1 to the height for printing th letters line 
               for(int i = 0; i < height; i++)
               {
                    string numberCell = string.Format("{0}  |", (i + 1).ToString());

                    Console.Write(numberCell);

                    // Adding 1 for the numbers color
                    for(int j = 0; j < width; j++)
                    {
                         bool isFlipped = i_MemoryCardGame.Board[i, j].IsFlipped;
                         char cellContent = isFlipped == true ? (char)i_MemoryCardGame.Board[i, j].CellContent : ' ';
                         string cellString = string.Format(" {0} |", cellContent.ToString());
                         Console.Write(cellString);
                    }

                    Console.Write(Environment.NewLine);
                    PrintBorder(width);
               }

               Console.Write(Environment.NewLine);
          }

          internal static void PrintLettersLine(int i_BoardWidth)
          {
               string lettersLine = "    ";

               Console.Write(lettersLine);

               for (int i = 0; i < i_BoardWidth; i++)
               {
                    string letterCell = string.Format(
                                        " {0}  ",
                                        (char)(i + 'A'));
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
               do
               {
                    Start();
               }
               while (IsGameStartingAgain() == true);
               Console.WriteLine("The game is over, thank you for playing");
          }

          internal static bool IsGameStartingAgain()
          {
               bool isValidAnswer = true,
                    isGameStartsOver = false;

               do
               {
                    Console.WriteLine("Would you like to play again? y/n");

                    string answer = Console.ReadLine();

                    if (answer != "y" && answer != "n")
                    {
                         Console.WriteLine("Invalid input, please try again");
                         isValidAnswer = false;
                    }
                    else
                    {
                         isValidAnswer = true;
                    }

                    isGameStartsOver = answer == "y";
               }
               while (isValidAnswer == false);

               return isGameStartsOver;
          }
     }
}
