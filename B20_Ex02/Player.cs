using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
     class Player
     {

          private string r_Name;
          private ePlayerType r_ETypeOfPlayer;
          private int m_Score; // NESHANE BASOF
          public enum ePlayerType
          {
               Human,
               Computer
          }

          public Player(string i_PlayerName, ePlayerType i_ETypeOfPlayer)
          {
               r_Name = i_PlayerName;
               r_ETypeOfPlayer = i_ETypeOfPlayer;
               m_Score = 0;

          }

          //Readonly optional
          public int Score
          {
               get => m_Score;
               set => m_Score = value;
          }
          //Readonly optional
          public string Name
          {
               get => r_Name;
               set => r_Name = value;
               
          }

          public ePlayerType enumPlayerType
          {
               get => r_ETypeOfPlayer;
               set => r_ETypeOfPlayer = value;
          }
     }
}





