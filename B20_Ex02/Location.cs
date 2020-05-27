using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
     public struct Location
     {
          public int m_Row;
          public int m_Col;

          public int Row 
          {
              get => m_Row;
              set => m_Row = value;
          }
          public int Col
          {
               get => m_Col;
               set => m_Col=value;
             
          }
     }
}
