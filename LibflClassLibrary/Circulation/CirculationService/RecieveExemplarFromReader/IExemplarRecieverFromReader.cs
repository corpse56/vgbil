using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation.CirculationService.RecieveExemplarFromReader
{
    public interface IExemplarRecieverFromReader
    {
        void RecieveBookFromReader(ExemplarBase exemplar, OrderInfo oi, BJUserInfo bjUser);
        void RecieveBookFromReader(ExemplarBase exemplar, OrderInfo oi, BJUserInfo bjUser, string circulationStatus);
        bool IsNeedToAskReaderForReserve(ExemplarBase exemplarBase, OrderInfo oi);
    }
}
