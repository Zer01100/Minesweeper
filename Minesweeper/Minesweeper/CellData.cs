using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class CellData
    {
        public int Value;
        public bool IsVisible;

        public CellData(int value)
        {
            Value = value;
            IsVisible = false;
        }
    }

}
