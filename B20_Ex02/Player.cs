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

               foreach (List<Location> item in io_CurrentGame.SeenCards.Values)
               {

                    if (item.Count == 2)
                    {
                        chosenLocation[0] = item[0];
                        chosenLocation[1] = item[1];
                        break;
                    }

               }

               if (chosenLocation[0] == null)
               {
                    Random randomObj = new Random();
                    int randomCard1 = randomObj.Next(io_CurrentGame.AvailableCards.Count - 1);
                    resultMove[0] = io_CurrentGame.AvailableCards[randomCard1];
                    chosenLocation[0] = resultMove[0].Location;


                    if(io_CurrentGame.SeenCards.TryGetValue(resultMove[0].CellContent, out List<Location> seenMatches))
                    {
                        if(seenMatches.Count > 0 && !(resultMove[0].Location.Equals(seenMatches[0])))
                        {
                            chosenLocation[1] = seenMatches[0];
                        }
                    }
                    else
                    {
                        int randomCard2 = randomObj.Next(io_CurrentGame.AvailableCards.Count);
                        while (randomCard2 == randomCard1)
                        {
                            randomCard2 = randomObj.Next(io_CurrentGame.AvailableCards.Count);
                        }

                        chosenLocation[1] = io_CurrentGame.AvailableCards[randomCard2].Location;
                    }
                    
               }

               resultMove[0] = io_CurrentGame.Board[chosenLocation[0].Value.Row, chosenLocation[0].Value.Col];
               resultMove[1] = io_CurrentGame.Board[chosenLocation[1].Value.Row, chosenLocation[1].Value.Col];
                
               return resultMove;
          }
     }
}





