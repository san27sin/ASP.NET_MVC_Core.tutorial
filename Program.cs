using System;
using System.Collections;
using System.Collections.Generic;
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
            Task1();
            Task2();
            Console.ReadKey(true);
        }

        public static void Task1()
        {
            DateTime start = DateTime.Now;

            float[] arr = new float[100_000_000];
            FillArray(arr);
            CalculateElOfArray(arr);

            DateTime finish = DateTime.Now;
            Console.WriteLine($"Инициализация массива заняла у нас:  {finish - start} сек.");
        }

        static public void SplitMidPoint<T>(T[] array, out T[] first, out T[] second)
        {
            Split(array, array.Length / 2, out first, out second);
        }

        static public void Split<T>(T[] array, int index, out T[] first, out T[] second)
        {
            first = array.Take(index).ToArray();
            second = array.Skip(index).ToArray();
        }

        public static void CalculateElOfArray(float[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (float)(arr[i] * Math.Sin(0.2f + i / 5) * Math.Cos(0.2f + i / 5) *
                    Math.Cos(0.4f + i / 2));
            }
        }

        public static void FillArray(float[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 1;
            }
        }

        static public void Task2()
        {
            DateTime start = DateTime.Now;

            var waitHandlers = new AutoResetEvent[2]
            {
                new AutoResetEvent(false),//имеет сигнальное и несигнальное состояние
                new AutoResetEvent(false)
            };


            /*устанавливаем состояние
             *в нашем случае мы устанавливаем объек в не сигнальном состоянии
             *при переводе объекта в сигнальное состояние, мы позволяем другим потокам продолжить работу
             *чтобы прервать работу потоков и дождаться сигнального состояния autoresetevent для этого надо вызвать метод waitone()
             *если у нас больше 2 потоков, то создаем массив из waitHandler[]
             */



            float[] arr = new float[100_000_000];
            FillArray(arr);
            SplitMidPoint(arr, out float[] first, out float[] second);

            Thread thread1 = new Thread(new ThreadStart(() =>
            {
                CalculateElOfArray(first);
                waitHandlers[0].Set();//переводим объект в сигнальное положение
            }));

            Thread thread2 = new Thread(new ThreadStart(() =>
            {
                CalculateElOfArray(second);
                waitHandlers[1].Set();//переводим объект в сигнальное состояние
            }));

            thread1.Start();
            thread2.Start();

            //waitHandler.WaitOne();когда на главном потоке мы вызываем этот метод, то переходим в режим ожидания, ждем пока наш поток не перейдет в сигнальное состояние, только для одного паралелльного потока
            //при нескольких потоков используем статический класс waithandler 
            
            WaitHandle.WaitAll(waitHandlers);//ждем завершение всех потоков
            Array.Copy(first, arr, first.Length);
            Array.Copy(second, 0, arr, arr.Length / 2, second.Length);

            DateTime finish = DateTime.Now;
            Console.WriteLine($"Инициализация массива заняла у нас:  {finish - start} сек.");
        }
    }
}
