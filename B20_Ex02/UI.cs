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
                    GenerateMemoryCards(memoryCards,boardMeasurements[0]*boardMeasurements[1]);

                    Game memoryGame = new Game(boardMeasurements, memoryCards, names, playersTypes);

                    HandleMoves(memoryGame);
               }
          }

          internal static void HandleMoves(Game io_MemoryGame)
          {
              Player currentPlayer = io_MemoryGame.Player1;
               
               while (io_MemoryGame.IsTheGameEnded() == false)
               {
                Location?[] cardLocation = new Location?[2];

                Console.WriteLine(String.Format("{0}'s turn{1}",
                       currentPlayer.Name,
                       Environment.NewLine));
                
                // player's type is a human
                if (currentPlayer.EPlayerType == Player.ePlayerType.Human)
                {
                    
                         // First Card
                         cardLocation[0] = GetHumanMove(io_MemoryGame);
                         // Optional reuseability
                         Ex02.ConsoleUtils.Screen.Clear();
                         io_MemoryGame.FlipCard(cardLocation[0]); 
                         PrintBoard(io_MemoryGame);

                         // Second Card
                         cardLocation[1] = GetHumanMove(io_MemoryGame);
                         
                    }
                    else
                {
                    cardLocation = currentPlayer.ComputerMove(io_MemoryGame.AvailableCards, io_MemoryGame.SeenCards);
                    io_MemoryGame.FlipCard(cardLocation[0]);
                    PrintBoard(io_MemoryGame);
                }
                CompleteMove(io_MemoryGame, currentPlayer, cardLocation[0], cardLocation[1]);
                // Updating current player
                currentPlayer = (currentPlayer == io_MemoryGame.Player1) ? io_MemoryGame.Player2 : io_MemoryGame.Player1;
               }

               DeclareWinner(io_MemoryGame.Player1,io_MemoryGame.Player2);
          }

          public static void DeclareWinner(params Player [] io_Players)
          {
              Console.WriteLine( String.Format(
                      "{0}'s Score:{1}{2}{3}'s Score{4}:",
                      io_Players[0].Name,
                      io_Players[0].ToString(),
                      Environment.NewLine,
                      io_Players[1].Name,
                      io_Players[1].Score.ToString()));

              if (io_Players[0].Score == io_Players[1].Score)
              {
                Console.WriteLine("The game ended with draw");  
              }
              else
              {
                  string winnerName = (io_Players[0].Score > io_Players[1].Score) ? io_Players[0].Name : io_Players[1].Name;
                  Console.WriteLine("{0} won",winnerName);
              }
          }
         
          internal static void CompleteMove(Game io_MemoryGame, Player io_CurrentPlayer, params Location?[] i_PairOfCards)
          {
               // Checking if there's a match and updating the status of card if necessary 
               bool isAMatch = io_MemoryGame.IsThereAMatch(io_CurrentPlayer, i_PairOfCards);
               Cell firstCard = io_MemoryGame.Board[i_PairOfCards[0].Value.Row,i_PairOfCards[0].Value.Col];
               Cell secondCard = io_MemoryGame.Board[i_PairOfCards[1].Value.Row,i_PairOfCards[1].Value.Col];

               // Flipping second card
               io_MemoryGame.FlipCard(i_PairOfCards[1]);
               Ex02.ConsoleUtils.Screen.Clear();
               PrintBoard(io_MemoryGame);

               if (isAMatch == false)
               {    // Screen suspension for 2 seconds
                    Thread.Sleep(2000);
                    // Flipping back cards
                    io_MemoryGame.FlipCard(i_PairOfCards[0]);
                    io_MemoryGame.FlipCard(i_PairOfCards[1]);
                    Ex02.ConsoleUtils.Screen.Clear();
                    PrintBoard(io_MemoryGame);
               }
               // Logic of data structures 
               io_MemoryGame.UpdateAvailableCards(isAMatch, firstCard,secondCard);
               io_MemoryGame.UpdateSeenCards(isAMatch, firstCard,secondCard);
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

                    // Check Whether the user pressed Q
                    if(cellString.Equals("Q"))
                    {
                        Environment.Exit(0);
                    }
                    

                    // Checking validity of the chosen cell
                    lengthIsValid = cellString.Length <= 2; 
                    isUpperCase   = lengthIsValid && char.IsUpper(cellString, 0);
                    column = (isUpperCase == true) ? (int)(cellString[0] - 'A') : -1; // Assigning -1 to column in case invalid
                    invalidInput = isUpperCase && char.IsDigit(cellString, 1)==false;

                    if(invalidInput == true)
                    {
                         Console.WriteLine("Invalid input location, Please try again.");
                    }
                    else
                    {
                         row = (int)(char.GetNumericValue(cellString[1]) - 1);
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
               do 
               {
                    Console.WriteLine("Please enter your name:");
                    name = Console.ReadLine();
                    inputIsValid = (name.Length > 0); 
                    if (inputIsValid == false)
                    {
                         Console.WriteLine("Name cannot be empty,please try again");
                         inputIsValid = false;
                    }

               }
               while(inputIsValid == false);

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
          // poorly implemented?
          internal static bool IsHeightOrWheightValid(out int io_Measurement)
          {
               bool inputIsValid = false;
               io_Measurement = 0;

               while (inputIsValid == false)
               {
                    inputIsValid = int.TryParse(Console.ReadLine(), out io_Measurement);
                    if (inputIsValid == false)
                    {
                         Console.WriteLine("The input must be a number, please try again");
                    }
                    else if(io_Measurement < 4) 
                    {
                         Console.WriteLine("The minimum value is 4, please try again");
                         inputIsValid = false;
                    }
                    else if (io_Measurement > 6)
                    {
                         Console.WriteLine("The maximum value is 6, please try again");
                         inputIsValid = false;
                    }
                    else
                    {
                         inputIsValid = true;
                    }
                    
               }

               return inputIsValid;
          }
          internal static void GetBoardMeasurementsFromUser(int[] io_BoardMeasurementArr)
          {

               bool inputIsValid = false;

               do
               { // Design isn't good 
                    do
                    {
                         Console.WriteLine("Please enter the height of the board");

                    }
                    while(IsHeightOrWheightValid(out io_BoardMeasurementArr[0]) == false);

                    {
                         Console.WriteLine("Please enter the width of the board");

                    }
                    while (IsHeightOrWheightValid(out io_BoardMeasurementArr[1]) == false);
                
                    if((io_BoardMeasurementArr[0] * io_BoardMeasurementArr[1]) % 2 != 0)
                    {
                         Console.WriteLine("The size of the board must be even");
                    }
                    else
                    {
                         inputIsValid = true;
                    }
               }
               while(inputIsValid == false);
          }
          internal static void GenerateMemoryCards(List<int> io_MemoryCards,int i_NumOfCards)
          {
               for (int i = 0; i < i_NumOfCards; i +=2)
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
               for (int i = 0; i < height; i++)
               {
                    String numberCell = String.Format("{0}  |",
                         (i + 1).ToString());

                    Console.Write(numberCell);

                    // Adding 1 for the numbers color
                    for (int j = 0; j < width; j++)
                    {
                         bool isFlipped = i_MemoryCardGame[i, j].IsFlipped;
                         char cellContent = ((isFlipped == true) ? (char)(i_MemoryCardGame[i, j].CellContent) : ' ');
                         String cellString = String.Format(" {0} |",
                              cellContent.ToString());

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
               do
               {
                    Start();
               }
               while (IsGameStartingAgain() == true);
          }

          internal static bool IsGameStartingAgain()
          {

               bool isValidAnswer    = true, 
                    isGameStartsOver = false;
               char answer           = 'n';

               do
               {
                    Console.WriteLine("Would you like to play again? y/n");

                   if ( (answer != 'y' && answer != 'n') || char.TryParse(Console.ReadLine(), out answer) == false)
                   {
                        Console.WriteLine("Invalid input, please try again");
                        isValidAnswer = false;
                   }
                   else
                   {
                        isValidAnswer = true;
                   }

                   isGameStartsOver = (answer == 'y');
               }
               while (isValidAnswer == false);

               return isGameStartsOver;
          }
          
     }
}
