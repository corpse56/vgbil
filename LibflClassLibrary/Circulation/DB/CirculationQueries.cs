using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Circulation.DB
{
    class CirculationQueries
    {

        public string INSERT_INTO_USER_BASKET
        {
            get
            {
                return "insert into Circulation..Basket (BookId,ReaderId,PutDate) values (@BookID,@IDReader, getdate())";
            }
        }

        public string GET_BASKET
        {
            get
            {
                return "select * from Circulation..Basket where ReaderId = @ReaderId";
            }
        }

        public string IS_EXISTS_IN_BASKET
        {
            get
            {
                return "select 1 from Circulation..Basket where ReaderId = @IDReader and BookId = @BookID";
            }
        }
        public string GET_ORDERS
        {
            get
            {
                return "select * from Reservation_R..ALISOrders where ReaderId = @ReaderId";
            }
        }

        public string DELETE_FROM_BASKET
        {
            get
            {
                return "delete from Circulation..Basket where ReaderId = @IDReader and BookId = @BookId;";
                       
            }
        }
        public string DELETE_FROM_BASKET_RESERVATION_O
        {
            get
            {
                return "delete from Reservation_O..Basket where IDREADER = @IDReader and IDMAIN = @idmain ";
            }
        }

        public string ELECTRONIC_ISSUE_COUNT
        {
            get
            {
                return //"select 1 from Reservation_R..ELISSUED where IDREADER = @IDReader" +
                       //" union all" +
                       " select 1 from Circulation..Orders where ReaderId = @IDReader and StatusName = 'Электронная выдача'";
            }
        }

        public string IS_ELECTRONIC_ISSUE_ALREADY_ISSUED
        {
            get
            {
                return //" select 1 from Reservation_R..ELISSUED where IDREADER = @IDReader and IDMAIN = @BookIdInt and BASE = @BASE" +
                       //" union all" +
                       " select 1 from Circulation..Orders where ReaderId = @IDReader and StatusName = 'Электронная выдача'" +
                       "     and BookId = @BookId";
            }
        }

        public string GET_BUSY_EXEMPLARS_COUNT
        {
            get
            {
                return " select * from Circulation..Orders " +
                       " where StatusName in ('Заказ сформирован','Сотрудник хранения подбирает книгу','На бронеполке','Выдано в зал','Выдано на дом','Электронная выдача') " +
                       " and BookId = @BookId";
            }
        }

        public string IS_TWENTYFOUR_HOURS_PAST_SINCE_RETURN
        {
            get
            {
                return " select * from Circulation..Orders A " +
                       " left join Circulation..OrdersFlow B on A.ID = B.OrderId " +
                       " where A.BookId = @BookId and A.ReaderId = @ReaderId and A.StatusName = 'Завершено'" +
                       " and B.StatusName = 'Электронная выдача'";
            }
        }

        public string NEW_ELECTRONIC_ORDER
        {
            get
            {
                return "insert into Circulation..Orders ( BookId,  ExemplarId,  ReaderId,  StatusName,           StartDate,  ReturnDate,                   Barcode,   Fund)" +
                       " values                         (@BookId,  0,          @ReaderId, 'Электронная выдача',  getdate(), DATEADD(day , 30 , getdate()), 'E00000000', @Fund  ); " +
                       " select SCOPE_IDENTITY() ";
            }
        }
        public string NEW_ORDER
        {
            get
            {
                return "insert into Circulation..Orders ( BookId,  ExemplarId,  ReaderId,  StatusName,           StartDate,  ReturnDate,                               Barcode,   Fund)" +
                       " values                         (@BookId,  @ExemplarId, @ReaderId, 'Заказ сформирован',   getdate(), DATEADD(day , @ReturnInDays , getdate()), @Barcode, @Fund  ); " +
                       " select SCOPE_IDENTITY() ";
            }
        }

        //здесь не вставляем статус 'Для возврата в хранение', потому что книга может быть на самом деле на месте, просто её не приняли.
        //но тогда надо не забывать для книг с таким статусом закрывать заказ и открывать новый.
        //и в программе хранения надо дать возможность проверить такие заказы.
        public string IS_ALREADY_ISSUED
        {
            get
            {
                return " select 1 from Circulation..Orders where ExemplarId = @ExemplarId " +
                       " and StatusName in ('Заказ сформирован','Сотрудник хранения подбирает книгу','На бронеполке','Выдано в зал','Выдано на дом')";
            }

        }

        public string IS_BOOK_ALREADY_ISSUED_TO_READER
        {
            get
            {
                return " select 1 from Circulation..Orders where ReaderId = @IDReader " +
                       " and BookId = @BookId";
            }
        }
    }
}
