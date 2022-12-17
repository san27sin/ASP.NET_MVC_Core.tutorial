using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_MVC_Core.tutorial
{
    public class Program
    {
        static void Main(string[] args)
        {
            Payment paymentFixed = new FixedPayment(5_000, 20);
            paymentFixed.employers.Add(new Employer("Alex", "Sinitsyn"));
            paymentFixed.employers.Add(new Employer("Pasha", "Kruglov"));
            Console.WriteLine(paymentFixed.ToString());

            Payment paymentPerHour = new PerHourPayment(300);
            paymentPerHour.employers.Add(new Employer("Kola", "Lisitsyn"));
            paymentPerHour.employers.Add(new Employer("Masha", "Davasha"));
            Console.WriteLine(paymentPerHour.ToString());

            Console.ReadKey();
        }

        public enum Time
        {
            hour,
            month
        }


        public class Employer
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public Employer(string name, string surname)
            {
                Name = name;
                Surname = surname;
            }
        }

        public abstract class Payment
        {

            public List<Employer> employers { get; } = new List<Employer>();
            public double Wage { get; }
            public Payment(double wage)
            {
                Wage = wage;
            }

            public abstract double CalculateMonthPayment();

            public override string ToString()
            {
                if(employers.Count == 0)
                    return String.Empty;

                var sb = new StringBuilder();
                foreach (var employer in employers)
                    sb.Append($"{employer.Name} {employer.Surname} / получил среднемесячную зарплату {CalculateMonthPayment().ToString()} руб.\n");
                return sb.ToString();
            }
        }

        public class FixedPayment : Payment
        {
            public int Days
            {
                get{ return _days; }
                private set
                {
                    if (_days < 0)
                        value = 0;
                    if (_days > 20)
                        value = 20;
                    else
                        value = _days;
                }
            }

            private int _days;

            public FixedPayment(double wage, int days) : base(wage)
            { _days = days; }

            public override double CalculateMonthPayment() => (double)(this.Wage * _days);
        }


        public class PerHourPayment : Payment
        {
            public PerHourPayment(double wage) : base(wage) { }
            public override double CalculateMonthPayment() => (double)(20.8 * 8  * Wage);
        }
    }
}

