using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks.BJExemplars;

namespace LibflClassLibrary.ALISAPI.ResponseObjects.Books
{
    public class BJExemplarSimpleViewer : IExemplarSimpleViewer
    {
        public ExemplarSimpleViewInfo GetExemplarSimpleView(ExemplarBase exemplar)
        {
            BJExemplarInfo bjExemplar = (BJExemplarInfo)exemplar;
            ExemplarSimpleViewInfo result = ViewFactory.GetBJExemplarSimpleView(bjExemplar);
            return result;
        }
    }
}
