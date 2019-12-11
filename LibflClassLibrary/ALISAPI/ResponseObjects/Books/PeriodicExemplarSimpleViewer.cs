using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.PeriodicBooks;

namespace LibflClassLibrary.ALISAPI.ResponseObjects.Books
{
    public class PeriodicExemplarSimpleViewer : IExemplarSimpleViewer
    {
        public ExemplarSimpleViewInfo GetExemplarSimpleView(ExemplarBase exemplar)
        {
            PeriodicExemplarInfo pExemplar = (PeriodicExemplarInfo)exemplar;
            ExemplarSimpleViewInfo result = ViewFactory.GetPeriodicExemplarSimpleView(pExemplar);
            return result;
        }
    }
}
