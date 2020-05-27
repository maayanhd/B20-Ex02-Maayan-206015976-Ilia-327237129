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
