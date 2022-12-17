using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ASP.NET_MVC_Core.tutorial
{
    public class Program
    {
        static void Main(string[] args)
        {
            Payment paymentFixed = new FixedPayment(5_000, 20);

            var employer = paymentFixed
                .CreateEmployer()
                .AddNameEmployer("Sasha")
                .AddMiddleNameEmployer("Romanovish")
                .AddSurnameEmployer("Sinitsyn")
                .AddSalaryEmployer()
                .EmployerJoinToGroup()
                .GetEmployer();


            Console.WriteLine(paymentFixed.ToString());

            Payment paymentPerHour = new PerHourPayment(300);
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
            public string MiddleName { get; set; }
            public string Surname { get; set; }
            public double Salary { get; set; }
        }

        public abstract class Payment
        {
            protected List<Employer> employers { get; } = new List<Employer>();

            protected Employer employer;

            public double Wage { get; }
            public Payment(double wage)
            {
                Wage = wage;
            }

            public abstract double CalculateMonthPayment();

            public Payment CreateEmployer()
            {
                employer = new Employer();
                return this;
            }

            public Payment AddNameEmployer(string Name)
            {
                if (employer != null && !String.IsNullOrEmpty(Name))
                    employer.Name = Name;

                return this;
            }

            public Payment AddMiddleNameEmployer(string MiddleName)
            {
                if (employer != null && !String.IsNullOrEmpty(MiddleName))
                    employer.MiddleName = MiddleName;

                return this;
            }

            public Payment AddSurnameEmployer(string Surname)
            {
                if (employer != null && !String.IsNullOrEmpty(Surname))
                    employer.Surname = Surname;

                return this;
            }

            public Payment AddSalaryEmployer()
            {
                if (employer != null)
                    employer.Salary = CalculateMonthPayment();

                return this;
            }

            public Payment EmployerJoinToGroup()
            {
                if (employer != null)
                    employers.Add(employer);

                return this;
            }

            public Employer GetEmployer()
            {
                if (employer == null)
                    return null;

                return employer;
            }

            public override string ToString()
            {
                if(employers.Count == 0)
                    return "Сотрудники не зарегистрированы!";

                var sb = new StringBuilder();
                foreach (var employer in employers)
                    sb.Append($"{employer.Name} {employer.Surname} / получил среднемесячную зарплату {employer.Salary.ToString()} руб.\n");
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

