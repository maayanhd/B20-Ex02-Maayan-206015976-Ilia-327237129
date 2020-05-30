using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace B20_Ex02
{
     class Player
     {

          private string m_Name;
          private ePlayerType m_ETypeOfPlayer;
          private int m_Score;

          public enum ePlayerType
          {
               Human,
               Computer
          }

          public Player(string i_PlayerName, ePlayerType i_ETypeOfPlayer)
          {
               m_Name = i_PlayerName;
               m_ETypeOfPlayer = i_ETypeOfPlayer;
               m_Score = 0;

          }

          //Readonly optional
          public int Score
          {
               get
               {
                    return m_Score;
               }
               set
               {
                    m_Score = value;
               }
          }
          //Readonly optional
          public string Name
          {
               get
               {
                    return m_Name;
               }
               set
               {
                    m_Name = value;
               }
          }

          public bool IsComputer()
          {
              return this.m_ETypeOfPlayer == ePlayerType.Computer;
          }
          public ePlayerType EPlayerType
          {
               get
               {
                    return m_ETypeOfPlayer;
               }
               set
               {
                    m_ETypeOfPlayer = value;
               }
          }

          public Cell[] ComputerMove(Game io_CurrentGame)
          {
               Location?[] chosenLocation = new Location?[2];
               Cell [] resultMove = new Cell[2];

               // checks if we saw 2 equal available cards
               foreach (List<Location> item in io_CurrentGame.SeenCards.Values)
               {

                    if (item.Count == 2)
                    {
                        chosenLocation[0] = item[0];
                        chosenLocation[1] = item[1];
                        break;
                    }

               }

               // choose 1 random card, and check if we saw available card that equals to the first 
               if (chosenLocation[0] == null)
               {
                    chosenLocation[0] = RandomizeAvailableLocation(io_CurrentGame.AvailableCards);
                    resultMove[0] = io_CurrentGame.Board[chosenLocation[0].Value.Row, chosenLocation[0].Value.Col];
                    chosenLocation[0] = resultMove[0].Location;


                    // and check if we saw available card that equals to the first 
                    if(io_CurrentGame.SeenCards.TryGetValue(resultMove[0].CellContent, out List<Location> seenMatches) && seenMatches.Count > 0 && (resultMove[0].Location.Equals(seenMatches[0])) == false) 
                    {
                            chosenLocation[1] = seenMatches[0];
                    }
                    else // chose the second card randomly aswell
                    {
                        chosenLocation[1] = RandomizeAvailableLocation(io_CurrentGame.AvailableCards);
                        while(chosenLocation[0].Equals(chosenLocation[1]))
                        {
                            chosenLocation[1] = RandomizeAvailableLocation(io_CurrentGame.AvailableCards);
                        }
                    }
                    
               }

               resultMove[0] = io_CurrentGame.Board[chosenLocation[0].Value.Row, chosenLocation[0].Value.Col];
               resultMove[1] = io_CurrentGame.Board[chosenLocation[1].Value.Row, chosenLocation[1].Value.Col];
                
               return resultMove;
          }

          public Location RandomizeAvailableLocation(List<Cell> io_AvailableCards)
          {

              Random randomObj = new Random();
              int randomIndex = randomObj.Next(io_AvailableCards.Count-1);

              return io_AvailableCards[randomIndex].Location;

          }
     }
}





