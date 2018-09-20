using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamic_example
{

    class IItem
    {
        //public virtual string GetView(IViewer viewer)
        //{
        //    return "base";
        //}
    }

    class Book : IItem
    {
        //public override string GetView(IViewer viewer)
        //{
        //    return viewer.View(this);
        //}
    }

    class Magazine : IItem
    {
        //public override string GetView(IViewer viewer)
        //{
        //    return viewer.View(this);
        //}

    }

    interface IViewer
    {
        string View(IItem item);
    }

    class FullView : IViewer
    {
        public string View(IItem item)
        {
            return "FullInterface";
        }
        public string View(Book item)
        {
            return "FullBook";
        }
        public string View(Magazine item)
        {
            return "FullMagazine";
        }

    }
    class ShortView : IViewer
    {
        public string View(IItem item)
        {
            return "shortInterface";
        }
        public string View(Book item)
        {
            return "shortBook";
        }
        public string View(Magazine item)
        {
            return "shortMagazine";
        }
    }

 
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<IItem> { new Book(), new Magazine(), new Book(), new IItem() };
            var full = new ShortView();
            foreach (var item in list)
            {
                Console.WriteLine(full.View((dynamic)item));
            }
            Console.ReadKey();
        }

        
    }
}
