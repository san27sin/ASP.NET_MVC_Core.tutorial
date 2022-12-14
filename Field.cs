using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASP.NET_MVC_Core.tutorial
{
    public class Field
    {
        public int Cols { get => field.GetLength(0); }
        public int Rows { get => field.GetLength(1); }

        public AutoResetEvent[] Handles { get => handles; }

        private char[,] field;

        private AutoResetEvent[] handles;

        public Field(int x, int y)
        {
            field = new char[x, y];

            handles = new AutoResetEvent[2]
            {
                new AutoResetEvent(false),
                new AutoResetEvent(false)
            };

            FillField();
        }

        private void FillField()
        {
            for (int y = 0; y < field.GetLength(0); y++)
            {
                for (int x = 0; x < field.GetLength(1); x++)
                {
                    field[y, x] = '.';
                }
            }
        }

        public void PrintField()
        {
            for (int y = 0; y < field.GetLength(0); y++)
            {
                for (int x = 0; x < field.GetLength(1); x++)
                {
                    Console.Write($"{field[y, x]} ");
                }
                Console.WriteLine();
            }
        }

        public bool Plant(int x, int y, char seed)
        {
            if (x < 0 || x >= field.GetLength(0) || y < 0 || y >= field.GetLength(1))
                return false;
            if (seed != 'X' && seed != '0')
                return false;
            if (field[x, y] == '.')
            {
                field[x, y] = seed;
                return true;
            }
            else
                return false;
        }
    }
}
