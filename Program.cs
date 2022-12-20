using System;
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
            IScan scanertype = new PDFScaner();
            var scaner = new Scaner();
        }
    }

    interface IScan
    {
        void Scan();
    }

    public class PDFScaner : IScan
    {
        public void Scan()
        {
            //Скан пдф
        }
    }

    public class DOCXScanner : IScan
    {
        public void Scan()
        {
            //Скан Docx
        }
    }

    public class QRCodeScanner : IScan
    {
        public void Scan()
        {
            //Скан QRCodeScanner
        }
    }

    public class Scaner
    {
        //Придумать событие которое через каждые 5 секунд показывает нам состояние ЦП и Оперативно памяти
        private IScan _scan;
        private static TimeSpan period = TimeSpan.FromSeconds(5);
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;



        public Scaner(IScan scan, string PathToFile)
        {
            _scan = scan;
            cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public void ScanFile()
        {
            var timer = new Timer( 
                x => Console.WriteLine(string.Format("CPU Value: {0}, ram value: {1}", cpu, ram)), 
                null, period, period);
            _scan.Scan();
        }
    }
}
