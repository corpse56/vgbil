using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation
{
    static class OrderTypes
    {
        public const string PaperVersion = "Бумажная версия";//на дом
        public const string InLibrary = "В библиотеке";//в зал
        public const string ElectronicVersion = "Электронная версия";//Электронно
        public const string ClarifyAccess = "Уточнить доступ";//
        public const string SelfOrder = "Самостоятельный заказ";//
        public const string NoActionProvided = "Действий для заказа не предусмотрено";//

    }
}
