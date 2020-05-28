using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace B20_Ex02
{
     class Game
     {
          private Player m_Player1                      = null;
          private Player m_Player2                      = null;
          private Cell[,] m_Board                       = null;
          private List<Cell> m_AvailableCards              = null;
          private Dictionary<int, List<Location>> m_SeenCards = null;
          // Constant values 
          const int k_HeightIndex                       = 0;
          const int k_WidthIndex                        = 1;

          public Game(int[] i_Measurements,List<int> i_Cards,string [] i_Names, Player.ePlayerType []i_PlayerTypes)
          {
               m_Board = new Cell[i_Measurements[k_HeightIndex],i_Measurements[k_WidthIndex]];
               initializeBoardAndAvailableCards(i_Cards);
               m_Player1.Name = i_Names[0];
               m_Player2.Name = i_Names[1];
               m_Player1.enumPlayerType = i_PlayerTypes[0];
               m_Player2.enumPlayerType = i_PlayerTypes[1];
          }
          private void initializeBoardAndAvailableCards(List<int> i_Cards)
          {
               int firstDimLength  = m_Board.GetLength(0);
               int secondDimLength = m_Board.GetLength(1);
               Random randObj      = new Random();

               for (int row = 0; row < firstDimLength; row++) 
               {
                    for (int col = 0; col < secondDimLength; col++) 
                    {
                         // Randomizing a card to put in the current location on the board 
                         int indexToAdd = randObj.Next(1, i_Cards.Count) - 1;
                         int availableCardsInd = 0;
                         bool isFlipped = false;

                         m_Board[row, col].CellContent = i_Cards[indexToAdd];
                         // Updating boolean state indicates the card is flipped 
                         m_Board[row, col].IsFlipped = false;
                         // Initializing location- Access to Location struct - using boxing or 
                        m_Board[row, col].m_Location.m_Row = row;
                        m_Board[row, col].m_Location.m_Col = col;
                        i_Cards.RemoveAt(indexToAdd);
                        // Initializing available cards storage
                        this.AvailableCards.Add(m_Board[row, col]);
                    }
               }
          }

          public void ManageAiStroage(bool i_IsAMatch)
          {
               UpdateAvailableCards(isAMatch);
               UpdateSeenCards(isAMatch);
          }

          public void UpdateAvailableCards(bool i_IsAMatch)
          {

          }
          public void UpdateSeenCards(bool i_IsAMatch)
          {

          }


          public bool IsThereAMatch(Player io_CurrentPlayer, params Location?[] twoLocations)
          {
               bool isAMatch = false;
               // Checking whether the player has found a pair of cards 
               // Need to implement == operator
               if (twoLocations[0].Equals(twoLocations[1]))
               {    
                    isAMatch = true;
                    // Updating score 
                    io_CurrentPlayer.Score++;
               }
               // Other wise, no need to update the score
               return isAMatch;
          }
          public bool IsTheGameEnded()
          {
               return (AvailableCards.Count == 0);
          }
          public Player Player1
          {
               get
               {
                    return m_Player1;
               }
               set
               {
                    m_Player1 = value;
               }
          }
          public Player Player2
          {
               get
               {
                    return m_Player2;
               }
               set
               {
                    m_Player2 = value;
               }
          }
          public Cell[,] Board
          {
               get
               {
                    return m_Board;
               }
               set  
               {
                    m_Board = value;
               }
          }
          public Cell this[int i, int j]
          {
               get
               {
                    return m_Board[i, j];
               }
               set
               {
                    m_Board[i, j] = value;
               }
          }
          public List<Cell> AvailableCards
          {
               get
               {
                    return m_AvailableCards;
               }
               set
               {
                    m_AvailableCards = value;
               }
          }

          public Dictionary<int, List<Location>> SeenCards
          {
               get
               {
                    return m_SeenCards;
               }
               set
               {
                    m_SeenCards = value;
               }
          }

          public void FlipCard(Location? i_CardLocation)
          {

               int i = i_CardLocation.Value.m_Col,
                   j = i_CardLocation.Value.m_Row;

               // Flipping Card by logic
               m_Board[i, j].IsFlipped = !m_Board[i, j].IsFlipped;
          }
     }
}


