using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
     struct Pair
     {
         public int m_row;
         public int m_col;

          public int row
          {
               get
               {
                    return m_row;
               }
               set
               {
                    m_row = value;
               }
          }
          public int col
          {
               get
               {
                    return m_col;
               }
               set
               {
                    m_col = value;
               }
          }
     }
}
