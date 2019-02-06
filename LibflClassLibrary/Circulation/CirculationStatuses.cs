using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation
{
    static class CirculationStatuses
    {
        public const string OrderIsFormed = "Заказ сформирован";
        public const string EmployeeLookingForBook = "Сотрудник хранения подбирает книгу";
        public const string InReserve = "На бронеполке";
        public const string IssuedInHall = "Выдано в зал";
        public const string IssuedAtHome = "Выдано на дом";
        public const string IssuedFromAnotherReserve = "Выдано с чужой бронеполки";
        public const string ForReturnToBookStorage = "Для возврата в хранение";
        public const string Finished = "Завершено";
        public const string Refusual = "Отказ";
        public const string ElectronicIssue = "Электронная выдача";
        public const string SelfOrder = "Самостоятельный заказ";
        public const string WaitingFirstIssue = "Ожидает выдачи";

    }

}
