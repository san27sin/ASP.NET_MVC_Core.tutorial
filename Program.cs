using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASP.NET_MVC_Core.tutorial
{
  
    public class Program
    {
        
        static void Main(string[] args)
        {
            Console.Write("Укажите размер поля по оси X: ");
            int x = Convert.ToInt32(Console.ReadLine());
            Console.Write("Укажите размер поля по оси Y: ");
            int y = Convert.ToInt32(Console.ReadLine());
            var field = new Field(x, y); 
            field.PrintField();


            ThreadPool.QueueUserWorkItem(Farmer0, field);
            ThreadPool.QueueUserWorkItem(FarmerX, field);

            WaitHandle.WaitAll(field.Handles);

            Console.WriteLine();
            field.PrintField();
            Console.ReadKey(true);
        } 


        public static void FarmerX(object o)
        {
            if (o!=null && o is Field)
            {
                var field = (Field)o;
                for(int y = 0; y < field.Rows;y++)
                {
                    for(int x = 0; x < field.Cols;x++)
                    {
                        if (field.Plant(x, y, 'X'))
                            Thread.Sleep(10);
                        else
                            continue;
                    }
                }
                field.Handles[0].Set();
            }
        }

        public static void Farmer0(object o)
        {
            if (o != null && o is Field)
            {
                var field = (Field)o;
                for(int x = field.Cols; x >= 0; x--)
                {
                    for(int y = field.Rows; y >= 0; y--)
                    {
                        if (field.Plant(x, y, '0'))
                            Thread.Sleep(10);
                        else
                            continue;
                    }
                }

                field.Handles[1].Set();
            }
        }
    }
}
