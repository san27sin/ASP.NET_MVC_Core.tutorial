using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASP.NET_MVC_Core.tutorial
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IScan scanertype = new PDFScaner();
            var scaner = new Scaner(scanertype, @"C:\Users\Alex\Desktop\Pushkin.txt");
            scaner.ScanFile();
            scaner.ConvertToBinary("C:\\Users\\Alex\\source\\repos\\result.dat");

            Console.ReadKey(true);
            scaner.Dispose();
        }
    }

    public interface IScan
    {
        byte[] Scan(string path);
    }

    public class PDFScaner : IScan
    {
        public byte[] Scan(string path)
        {
            //Скан пдф
            return new byte[] { };
        }
    }

    public class DOCXScanner : IScan
    {

        public byte[] Scan(string path)
        {
            if (!File.Exists(path))
                return null;

            FileInfo fileInfo = new FileInfo(path);

            Console.WriteLine($"Имя файла: {fileInfo.Name}");
            Console.WriteLine($"Время создания: {fileInfo.CreationTime}");
            Console.WriteLine($"Размер: {fileInfo.Length}");
            return File.ReadAllBytes(path);
        }
    }

    public class QRCodeScanner : IScan
    {
        public byte[] Scan(string path)
        {
            //Скан QRCodeScanner
            return new byte[] { };
        }
    }

    public class Scaner : IDisposable
    {
        //Придумать событие которое через каждые 5 секунд показывает нам состояние ЦП и Оперативно памяти
        private IScan _scan;
        private static TimeSpan _period = TimeSpan.FromSeconds(5);
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _ramCounter;
        private string _path;
        private byte[] _bytes;

        public Scaner(IScan scan, string PathToFile)
        {
            _scan = scan;
            _path = PathToFile;
            _cpuCounter = new PerformanceCounter();
            _cpuCounter.CategoryName = "Processor";
            _cpuCounter.CounterName = "% Processor Time";
            _cpuCounter.InstanceName = "_Total";
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public void ScanFile()
        {
            var timer = new Timer(
                x => Console.WriteLine(string.Format($"CPU Value: {_cpuCounter.NextValue()}, ram value: {_ramCounter.NextValue()}"),
                null, _period, _period));
            _bytes = _scan.Scan(_path);
        }

        public bool ConvertToBinary(string pathToSave)
        {
            if (!Directory.Exists(Path.GetDirectoryName(pathToSave)))
                return false;

            using (BinaryWriter reader = new BinaryWriter(File.Open(pathToSave, FileMode.OpenOrCreate)))
            {
                reader.Write(_bytes);
            }

            return true;
        }

        public void Dispose()
        {
            _cpuCounter.Dispose();
            _ramCounter.Dispose();
        }
    }

}
