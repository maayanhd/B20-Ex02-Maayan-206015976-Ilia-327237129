using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
     class Cell
     {
          private int m_CellContent;
          private bool m_IsFlipped;
          public Location m_Location;

          public Cell(int i_CellContent, bool i_IsFlipped, params int[] i_CardCoordinates)
          {
               m_CellContent  = i_CellContent;
               m_IsFlipped    = i_IsFlipped;
               m_Location.Row = i_CardCoordinates[0];
               m_Location.Col = i_CardCoordinates[1];

          }
          public int CellContent
          {
               get
               {
                    return m_CellContent;
               }
               set
               {
                    m_CellContent = value;
               }
          }
          public bool IsFlipped
          {
               get
               {
                    return m_IsFlipped;
               }
               set
               {
                    m_IsFlipped = value;
               }
          }
          public Location Location
          {
               get
               {
                    return m_Location;
               }
               set
               {
                    m_Location = value;
               }
          }
     }
}
