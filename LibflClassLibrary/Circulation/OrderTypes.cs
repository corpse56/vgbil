using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation
{
    public struct OrderTypes
    {
        //public static readonly ListEntry PaperVersion               = new ListEntry(1, "Бумажная версия");
        //public static readonly ListEntry InLibrary                  = new ListEntry(2, "В библиотеке");
        //public static readonly ListEntry ElectronicVersion          = new ListEntry(3, "Электронная версия");
        //public static readonly ListEntry ClarifyAccess              = new ListEntry(4, "Уточнить доступ");
        //public static readonly ListEntry SelfOrder                  = new ListEntry(5, "Самостоятельный заказ");
        //public static readonly ListEntry NoActionProvided           = new ListEntry(6, "Действий для заказа не предусмотрено");

        public struct PaperVersion
        {
            public const int Id = 1;
            public const string Value = "Бумажная версия";
        }
        public struct InLibrary
        {
            public const int Id = 2;
            public const string Value = "В библиотеке";
        }
        public struct ElectronicVersion
        {
            public const int Id = 3;
            public const string Value = "Электронная версия";
        }

        public struct ClarifyAccess
        {
            public const int Id = 4;
            public const string Value = "Уточнить доступ";
        }

        public struct SelfOrder
        {
            public const int Id = 5;
            public const string Value = "Самостоятельный заказ";
        }

        public struct NoActionProvided
        {
            public const int Id = 6;
            public const string Value = "Действий для заказа не предусмотрено";
        }
        public struct OrderProhibited
        {
            public const int Id = 7;
            public const string Value = "Недостаточно прав для заказа бумажной версии";
        }




        public static Dictionary<int,string> ListView
        {
            get
            {
                return new Dictionary<int, string>()
                {
                    { PaperVersion.Id,      PaperVersion.Value },
                    { InLibrary.Id,         InLibrary.Value },
                    { ElectronicVersion.Id, ElectronicVersion.Value },
                    { ClarifyAccess.Id,     ClarifyAccess.Value },
                    { SelfOrder.Id,         SelfOrder.Value },
                    { NoActionProvided.Id,  NoActionProvided.Value },
                    { OrderProhibited.Id,  OrderProhibited.Value }
                };
            }
        }
    }
}
