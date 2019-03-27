using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation
{
    public static class CirculationStatuses
    {
        //public static readonly ListEntry OrderIsFormed              = new ListEntry(1, "Заказ сформирован");
        //public static readonly ListEntry EmployeeLookingForBook     = new ListEntry(2, "Сотрудник хранения подбирает книгу");
        //public static readonly ListEntry InReserve                  = new ListEntry(3, "На бронеполке");
        //public static readonly ListEntry IssuedInHall               = new ListEntry(4, "Выдано в зал");
        //public static readonly ListEntry IssuedAtHome               = new ListEntry(5, "Выдано на дом");
        //public static readonly ListEntry IssuedFromAnotherReserve   = new ListEntry(6, "Выдано с чужой бронеполки");
        //public static readonly ListEntry ForReturnToBookStorage     = new ListEntry(7, "Для возврата в хранение");
        //public static readonly ListEntry Finished                   = new ListEntry(8, "Завершено");
        //public static readonly ListEntry Refusual                   = new ListEntry(9, "Отказ");
        //public static readonly ListEntry ElectronicIssue            = new ListEntry(10, "Электронная выдача");
        //public static readonly ListEntry SelfOrder                  = new ListEntry(11, "Самостоятельный заказ");
        //public static readonly ListEntry WaitingFirstIssue          = new ListEntry(12, "Ожидает выдачи");
        public static class OrderIsFormed
        {
            public const int Id = 1;
            public const string Value = "Заказ сформирован";
        }
        public static class EmployeeLookingForBook
        {
            public const int Id = 2;
            public const string Value = "Сотрудник хранения подбирает книгу";
        }
        public static class InReserve
        {
            public const int Id = 3;
            public const string Value = "На бронеполке";
        }
        public static class IssuedInHall
        {
            public const int Id = 4;
            public const string Value = "Выдано в зал";
        }
        public static class IssuedAtHome
        {
            public const int Id = 5;
            public const string Value = "Выдано на дом";
        }
        public static class IssuedFromAnotherReserve
        {
            public const int Id = 6;
            public const string Value = "Выдано с чужой бронеполки";
        }
        public static class ForReturnToBookStorage
        {
            public const int Id = 7;
            public const string Value = "Для возврата в хранение";
        }
        public static class Finished
        {
            public const int Id = 8;
            public const string Value = "Завершено";
        }
        public static class Refusual
        {
            public const int Id = 9;
            public const string Value = "Отказ";
        }
        public static class ElectronicIssue
        {
            public const int Id = 10;
            public const string Value = "Электронная выдача";
        }
        public static class SelfOrder
        {
            public const int Id = 11;
            public const string Value = "Самостоятельный заказ";
        }
        public static class WaitingFirstIssue
        {
            public const int Id = 12;
            public const string Value = "Ожидает выдачи";
        }
        //фейковый статус. никогда не присваивается, только для отметки того, что заказ продлевали. Вполне вписывается в таблицу OrdersFlow. 
        //поэтому не будем городить лишних сущностей.
        public static class Prolonged
        {
            public const int Id = 13;
            public const string Value = "Продлено";
        }


        public static Dictionary<int, string> ListView
        {
            get
            {
                return new Dictionary<int, string>()
                {
                    { OrderIsFormed.Id, OrderIsFormed.Value },
                    { EmployeeLookingForBook.Id, EmployeeLookingForBook.Value },
                    { InReserve.Id, InReserve.Value },
                    { IssuedInHall.Id, IssuedInHall.Value },
                    { IssuedAtHome.Id, IssuedAtHome.Value },
                    { IssuedFromAnotherReserve.Id, IssuedFromAnotherReserve.Value },
                    { ForReturnToBookStorage.Id, ForReturnToBookStorage.Value },
                    { Finished.Id, Finished.Value },
                    { Refusual.Id, Refusual.Value },
                    { ElectronicIssue.Id, ElectronicIssue.Value },
                    { SelfOrder.Id, SelfOrder.Value },
                    { WaitingFirstIssue.Id, WaitingFirstIssue.Value },
                };
            }
        }


    }




    public class ListEntry
    {

        public ListEntry(int Id, string Value)
        {
            this.Id = Id;
            this.Value = Value;
        }
        public int Id { get; set; }
        public string Value { get; set; }
    }

}
