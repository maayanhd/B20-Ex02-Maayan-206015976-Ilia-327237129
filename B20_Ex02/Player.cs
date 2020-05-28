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

          public Location?[] ComputerMove(List<Cell> io_AvailableCards, Dictionary<int, List<Location>> io_SeenCards)
          {
               Location?[] resultMove = new Location?[2];


               foreach (List<Location> item in io_SeenCards.Values)
               {

                    if (item.Count == 2)
                    {
                         resultMove[0] = item[0];
                         resultMove[1] = item[1];
                    }

               }

               if (resultMove[0] == null)
               {
                    Random randomObj = new Random();
                    int randomCard1 = randomObj.Next(io_AvailableCards.Count - 1);
                    Cell chosenCard = io_AvailableCards[randomCard1];
                    resultMove[0] = chosenCard.Location;


                    if(io_SeenCards.TryGetValue(chosenCard.CellContent, out List<Location> seenMatches))
                    {
                        if(seenMatches.Count > 0 && !(chosenCard.Location.Equals(seenMatches[0])))
                        {
                            resultMove[1] = seenMatches[0];
                        }
                    }
                    else
                         {
                              int randomCard2 = randomObj.Next(io_AvailableCards.Count);
                              while (randomCard2 == randomCard1)
                              {
                                   randomCard2 = randomObj.Next(io_AvailableCards.Count);
                              }

                              resultMove[1] = io_AvailableCards[randomCard2].Location;
                         }
                    
               }

               return resultMove;
          }
     }
}





