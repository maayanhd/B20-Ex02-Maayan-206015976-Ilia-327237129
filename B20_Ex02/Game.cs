using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace B20_Ex02
{
     class Game
     {
          private Player m_Player1;
          private Player m_Player2;
          private Cell[,] m_Board;
          const int k_HeightIndex= 0;
          const int k_WidthIndex= 1;
          public Game(int[] i_Measurements,List<int> i_Cards,string [] i_Names, Player.ePlayerType []i_PlayerTypes)
          {
               m_Board = new Cell[i_Measurements[k_HeightIndex],i_Measurements[k_WidthIndex]];
               initializeBoard(i_Cards);
               m_Player1.Name = i_Names[0];
               m_Player2.Name = i_Names[1];
               m_Player1.enumPlayerType = i_PlayerTypes[0];
               m_Player2.enumPlayerType = i_PlayerTypes[1];
          }
          private void initializeBoard(List<int> i_Cards)
          {
               int firstDimLength = m_Board.GetLength(0);
               int secondDimLength = m_Board.GetLength(1);
               Random randObj = new Random();
               for (int row = 0; row < firstDimLength; row++) 
               {
                    for (int col = 0; col < secondDimLength; col++) 
                    {
                         // Randomizing a card to put in the current location on the board 
                         int indexToAdd = randObj.Next(1, i_Cards.Count) - 1;
                         m_Board[row, col].CellContent = i_Cards[indexToAdd];
                         // Updating boolean state indicates the card is flipped 
                         m_Board[row, col].IsFlipped = false;
                         // Initializing location- Access to Pair struct - using boxing or 
                        m_Board[row, col].m_Location.m_Row = row;
                        m_Board[row, col].m_Location.m_Col = col;
                        i_Cards.RemoveAt(indexToAdd);
                    }
               }
          }
          public bool IsAMatch(UI.eCurrentNumPlayer eCurrentPlayer, params Pair[] twoLocations)
          {
               bool isAMatch = false;
               // Checking whether the player has found a pair of card 
               // Need to implement == operator
               if (twoLocations[0].Equals(twoLocations[1]))
               {    
                    isAMatch = true;
                    // Updating score 
                    // We need to find abbreviation 
                    if (eCurrentPlayer == UI.eCurrentNumPlayer.Player1)
                    {
                         m_Player1.Score++;
                    }
                    else
                    {
                         m_Player2.Score++;
                    }
               }
               // Other wise, no need to update the score
               return isAMatch;
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
     }
}


