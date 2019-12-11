using LibflClassLibrary.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ALISAPI.ResponseObjects.Books
{
    public interface IExemplarSimpleViewer
    {
        ExemplarSimpleViewInfo GetExemplarSimpleView(ExemplarBase exemplar);
    }
}
