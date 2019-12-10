using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.PeriodicBooks;
using LibflClassLibrary.Circulation.CirculationService.RecieveExemplarFromReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace LibflClassLibrary.Circulation.CirculationService.RecieveExemplarFromReader
{
    //возможные классы изданий для периодики
//NULL

//label12
//Для библ.описания
//Для выдачи
//Для выставки
//Для длительного пользования
//Для реставрации
//Для служебного пользования
//Резервная копия
//Удалён
    class PeriodicExemplarRecieverFromReader : IExemplarRecieverFromReader
    {
        //возвращаем 1 - значит нужно спросить на бронеполку будет класть или для возврата в хранение
        //возвращаем 0 - значит либо книга в зал возвращается, либо из дома сдаёт, тоже для возврата в хранение
        public int RecieveBookFromReader(ExemplarBase exemplarBase, OrderInfo oi, BJUserInfo bjUser)
        {
            PeriodicExemplarInfo exemplar = (PeriodicExemplarInfo)exemplarBase;
            CirculationInfo ci = new CirculationInfo();
            ci.ChangeOrderStatusReturn(bjUser, oi.OrderId, CirculationStatuses.Finished.Value);

            if (oi.StatusCode == CirculationStatuses.IssuedInHall.Id)//в зал?
            {
                if (exemplar.PublicationClass == "Для длительного пользования")
                {
                    ci.ChangeOrderStatusReturn(bjUser, oi.OrderId, CirculationStatuses.Finished.Value);
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (exemplar.Fields["899$a"].ToString().ToLower().Contains("книгохранен"))
                {
                    ci.ChangeOrderStatusReturn(bjUser, oi.OrderId, CirculationStatuses.ForReturnToBookStorage.Value);
                }
                else
                {
                    ci.ChangeOrderStatusReturn(bjUser, oi.OrderId, CirculationStatuses.Finished.Value);
                }
            }
            return 0;

        }
    }
}
