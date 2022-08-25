using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Analysis
{
    public class RowCol
    {
        public int row;
        public int col;

        public RowCol(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
    public static class ConvertPosition
    {
        /* Преобразование индекса к столбцу и строке. Строки и столбцы нумеруются с 1*/
        public static RowCol IndexToRowCol(string str, int index)
        {
            if ((index >= str.Length) || (index < 0))
                return new RowCol(-1, -1);

            RowCol rc = new RowCol(1, 1);

            for (int i = 0; i < index; i++)
            {
                rc.col++;
                if (str[i] == '\n')
                {
                    rc.row++;
                    rc.col = 1;
                }
            }
            return rc;
        }
    }
}
