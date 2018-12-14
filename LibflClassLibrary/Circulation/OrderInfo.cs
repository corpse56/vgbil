using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Circulation
{
    /// <summary>
    /// Статусы книговыдачи
    /// </summary>
    /// 
    /// Книга в корзине(когда читатель положил книги в корзину и авторизовался)
    /// Заказ сформирован(когда читатель нажал кнопку заказать)
    /// Сотрудник хранения подбирает книгу(когда сотрудник хранения распечатал требование)
    /// На бронеполке(когда книга поступила на кафедру и сотрудник кафедры её принял)
    /// Выдано в зал
    /// Выдано на дом
    /// Выдано с чужой бронеполки
    /// Для возврата в хранение(когда читатель сдал книгу окончательно, а не на бронеполку)
    /// Завершено
    /// Отказ
    /// Электронная выдача

    public class OrderInfo
    {
        public BookSimpleView Book;
        public string BookId { get; set; }
        public int ExemplarId { get; set; }
        public int ReaderId { get; set; }
        public string StatusName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? FactReturnDate { get; set; }
        public int AnotherReaderId { get; set; }
        public string IssueDep { get; set; }
        public string ReturnDep { get; set; }
        public int OrderId { get; set; }

    }
}
