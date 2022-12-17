using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_MVC_Core.tutorial
{
    /*Придумайте небольшое приложение консольного типа, который берет различные Json
структуры (предположительно из разных веб сервисов), олицетворяюющие товар в магазинах.
Структуры не похожи друг на друга, но вам нужно их учесть, сделать универсально. Структуры
на ваше усмотрение.*/

    public class Program
    {
        static void Main(string[] args)
        {
            Item box = new Box("коробка", Guid.NewGuid(), "большая", "красный");
            Item knife = new Knife("нож", Guid.NewGuid(), "15 см",0.4, "сталь");

            var goods = new Goods();
            goods.Add(box);
            goods.Add(knife);

            var item1 = goods.Get(0);
            var item2 = goods.Get(1);
        }
    }

    public class Goods
    {
        private List<Item> items = new List<Item>();

        public Goods() { }

        public Goods(Item item)
        {
            items.Add(item);
        }

        public void Add(Item item)
        {
            items.Add(item);
        }

        public void Remove(Item item)
        {
            items.Remove(item);
        }
        
        public Item Get(int index)
        {
            if (index < 0 || index >= items.Count)
                throw new IndexOutOfRangeException();

            switch (items[index].GetType().Name)
            {
                case "Box":
                    return items[index] as Box;
                case "Knife":
                    return items[index] as Knife;
                default:
                    return items[index];
            }
        }
        
    }




    public interface IMovement
    {
        void Move();
    }

    public interface IFight
    {
        void Protect();
    }


    public abstract class Item
    {
        public string Name { get; set; }
        public Guid ItemNumber { get; set; }

        public Item (string name, Guid itemNumber)
        {
            Name = name;
            ItemNumber = itemNumber;
        }
    }

    public sealed class Box : Item, IMovement
    {
        public string Size { get; set; }
        public string Color { get; set; }
        public Box(string name, Guid item, string size, string color) 
            : base(name,item)
        {
            Size = size;
            Color = color;
        }

        public void Move()
        {
            Console.WriteLine("move box!");
        }
    }

    public sealed class Knife : Item, IFight
    {
        public double Sharp { get; set; }
        public string Material { get; set; }
        public Knife(string name, Guid item, string length, double sharp, string material)
            : base(name, item)
        {
            Sharp = sharp;
            Material = material;
        }

        public void Protect()
        {
            Console.WriteLine("Protect from abuser");
        }
    }

    public class KnifeAdapter
    {
        private IMovement _movement;

        public KnifeAdapter(IMovement movement)
        {
            _movement = movement;
        }

        public void Protect()
        {
            _movement.Move();
        }

    }
}
