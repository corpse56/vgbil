using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visitor_example
{
    class IItem
    {
        //public virtual string GetView(IViewer viewer)
        //{
        //    return "base";
        //}
        public virtual string AcceptViewVisitor(IItemVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }

    class Book : IItem
    {
        //public override string GetView(IViewer viewer)
        //{
        //    return viewer.View(this);
        //}
        public override string AcceptViewVisitor(IItemVisitor visitor)
        {
            return visitor.Visit(this);
        }


    }

    class Magazine : IItem
    {
        //public override string GetView(IViewer viewer)
        //{
        //    return viewer.View(this);
        //}
        public override string AcceptViewVisitor(IItemVisitor visitor)
        {
            return visitor.Visit(this);
        }

    }

    interface IItemVisitor
    {
        string Visit(IItem item);
        string Visit(Book item);
        string Visit(Magazine item);

    }

    class FullViewVisitor : IItemVisitor
    {
        public string Visit(IItem item)
        {
            return "IItem Full View";
        }
        public string Visit(Book item)
        {
            return "Book Full View";
        }
        public string Visit(Magazine item)
        {
            return "Magazine Full View";
        }
    }
    class ShortViewVisitor : IItemVisitor
    {
        public string Visit(IItem item)
        {
            return "IItem Short View";
        }
        public string Visit(Book item)
        {
            return "Book Short View";
        }
        public string Visit(Magazine item)
        {
            return "Magazine Short View";
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<IItem> { new Book(), new Magazine(), new Book(), new IItem() };
            var full = new FullViewVisitor();
            var Short = new ShortViewVisitor();
            foreach (var item in list)
            {
                Console.WriteLine(item.AcceptViewVisitor(Short));
            }
            Console.ReadKey();


        }
    }
}
