using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using Utilities;

namespace LibflClassLibrary.Circulation.CirculationService.RecieveExemplarFromReader
{
    class BJExemplarRecieverFromReader : IExemplarRecieverFromReader
    {
        public bool IsNeedToAskReaderForReserve(ExemplarBase exemplarBase, OrderInfo oi)
        {
            BJExemplarInfo exemplar = (BJExemplarInfo)exemplarBase;
            if (oi.StatusCode == CirculationStatuses.IssuedInHall.Id)
            {
                if (exemplar.PublicationClass == "ДП")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;

        }

        public void RecieveBookFromReader(ExemplarBase exemplarBase, OrderInfo oi, BJUserInfo bjUser)
        {
            BJExemplarInfo exemplar = (BJExemplarInfo)exemplarBase;
            CirculationInfo ci = new CirculationInfo();

            if (bjUser.Login.ToLower().In("station1", "station2", "station3", "station4"))
            {
                if (exemplar.Fields["899$a"].ToString().ToLower().Contains("книгохранен"))
                {
                    ci.ChangeOrderStatusReturn(bjUser, oi.OrderId, CirculationStatuses.ForReturnToBookStorage.Value);
                }
                else
                {
                    ci.ChangeOrderStatusReturn(bjUser, oi.OrderId, CirculationStatuses.Finished.Value);
                }
                return;
            }

            if (oi.StatusCode == CirculationStatuses.IssuedInHall.Id)
            {
                if (exemplar.Fields["921$c"].ToString() == "ДП")
                {
                    if ((exemplar.Fields["899$a"].ToString() != bjUser.SelectedUserStatus.DepName) &&
                        !(bjUser.Login.ToLower().In("station1", "station2", "station3", "station4")))
                    {
                        throw new Exception("C022");
                    }
                    ci.ChangeOrderStatusReturn(bjUser, oi.OrderId, CirculationStatuses.Finished.Value);
                }
                else
                {
                    //невозможная ситуация. это условие уже проверено при вызове IsNeedToAskReaderForReserve
                    //Поэтому сюда не должно попасть. как это обойти хороший вопрос
                    throw new Exception("C029");
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
        }

        public void RecieveBookFromReader(ExemplarBase exemplar, OrderInfo oi, BJUserInfo bjUser,string circulationStatus)
        {
            CirculationInfo ci = new CirculationInfo();
            ci.ChangeOrderStatusReturn(bjUser, oi.OrderId, circulationStatus);
        }

    }
}
