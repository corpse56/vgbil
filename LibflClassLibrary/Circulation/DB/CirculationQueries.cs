using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Circulation.DB
{
    class CirculationQueries
    {
        internal string GET_ORDERS_FOR_STORAGE
        {
            get
            {
                return  " select A.*, B.Refusual " +
                        " from Circulation..Orders A " +
                        " left join OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName" +
                        " where A.StatusName in ('Заказ сформирован','Сотрудник хранения подбирает книгу')";
            }
        }
        internal string GET_ORDERS_HISTORY_FOR_STORAGE
        {
            get
            {
                return " select top 300 A.*,B.Refusual from Circulation..Orders A " +
                       " left join OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName " +
                       " where A.StatusName not in ('Заказ сформирован','Сотрудник хранения подбирает книгу', 'Электронная выдача' )";
            }
        }

        internal string GET_ORDER
        {
            get
            {
                return " select A.*,B.Refusual from Circulation..Orders A " +
                        " left join OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName" +
                        " where A.ID = @OrderId";
            }
        }

        public string DELETE_ORDER
        {
            get
            {
                return "update Circulation..Orders set StatusName = @StatusName where ID = @OrderId";
            }
        }

        public string INSERT_INTO_USER_BASKET
        {
            get
            {
                return "insert into Circulation..Basket (BookId,ReaderId,AlligatBookId,PutDate) values (@BookID,@IDReader, @AlligatBookId, getdate())";
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
                return " select A.*,B.Refusual from Circulation..Orders A " +
                       " left join OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName" +
                       " where ReaderId = @ReaderId and A.StatusName not in ('Завершено', 'Для возврата в хранение')";
            }
        }
        public string GET_ORDERS_HISTORY
        {
            get
            {
                return "select * from Circulation..Orders where ReaderId = @ReaderId and StatusName in ('Завершено', 'Для возврата в хранение')";
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
                return "insert into Circulation..Orders ( BookId,  ExemplarId,  ReaderId,  StatusName,           StartDate,  ReturnDate,                   Barcode,   Fund, IssueDate)" +
                       " values                         (@BookId,  0,          @ReaderId, @StatusName,  getdate(), DATEADD(day , 30 , getdate()), 'E00000000', @Fund  ,  getdate()); " +
                       " select SCOPE_IDENTITY() ";
            }
        }
        public string NEW_ORDER
        {
            get
            {
                return "insert into Circulation..Orders ( BookId,  ExemplarId,  ReaderId,  StatusName,    StartDate,        ReturnDate,                        Barcode,   Fund,   AlligatBookId,   IssuingDepId)" +
                       " values                         (@BookId,  @ExemplarId, @ReaderId, @StatusName,   getdate(), DATEADD(day , @ReturnInDays , getdate()), @Barcode, @Fund,  @AlligatBookId,   @IssuingDepId ); " +
                       " select SCOPE_IDENTITY() ";
            }
        }
        public string CHANGE_ORDER_STATUS
        {
            get
            {
                return " BEGIN TRANSACTION; " +
                       " insert into Circulation..OrdersFlow (OrderId, StatusName, Changed,  Changer,    DepartmentId, Refusual ) " +
                       " values                            (@OrderId, @StatusName,getdate(), @Changer, @DepartmentId, @Refusual );" +
                       " update Circulation..Orders set StatusName = @StatusName where ID = @OrderId;" +
                       " COMMIT; ";
            }
        }

        //здесь не вставляем статус 'Для возврата в хранение', потому что книга может быть на самом деле на месте, просто её не приняли.
        //но тогда надо не забывать для книг с таким статусом закрывать заказ и открывать новый.
        //и в программе хранения надо дать возможность проверить такие заказы.
        //НУ НАХЕР. Просто не будем давать заказывать.
        public string IS_ALREADY_ISSUED
        {
            get
            {
                return " select 1 from Circulation..Orders where ExemplarId = @ExemplarId " +
                       " and StatusName in ('Заказ сформирован','Сотрудник хранения подбирает книгу','На бронеполке','Выдано в зал','Выдано на дом', 'Ожидает выдачи', 'Для возврата в хранение')";
            }

        }

        public string IS_BOOK_ALREADY_ISSUED_TO_READER
        {
            get
            {
                return " select 1 from Circulation..Orders where ReaderId = @IDReader " +
                       " and BookId = @BookId and StatusName not in ('Завершено','Для возврата в хранение')";
            }
        }

        public string ORDER_TIMES_PROLONGED
        {
            get
            {
                return " select * from Circulation..OrdersFlow where OrderId = @orderId and StatusName = @StatusName";
            }
        }

        public string PROLONG_ORDER
        {
            get
            {
                return " begin transaction; " +
                        " update Circulation..Orders set ReturnDate = dateadd(day, @days, ReturnDate) where ID = @orderId; " +
                       " insert into Circulation..OrdersFlow (OrderId, StatusName,  Changed,  Changer,    DepartmentId, Refusual ) " +
                       " values                              (@OrderId, @StatusName,getdate(),    1  ,      2033,           null ); " +
                       " commit;";
            }
        }

        public string GET_LITRES_ACCOUNT
        {
            get
            {
                return " select * from LITRES..ACCOUNTS where IDREADER = @ReaderId";
            }
        }

        public string ASSIGN_LITRES_ACCOUNT
        {
            get
            {
                return " update LITRES..ACCOUNTS set IDREADER = @ReaderId, ASSIGNED=getdate() where ID = @AccountId ";
            }
        }

        public string GET_FIRST_FREE_LITRES_ACCOUNT
        {
            get
            {
                return " select top 1 ID from LITRES..ACCOUNTS where IDREADER is null order by ID ";
            }
        }

        public string REFUSE_ORDER
        {
            get
            {
                return " begin transaction; " +
                        " update Circulation..Orders set StatusName = @StatusName where ID = @orderId; " +
                       " insert into Circulation..OrdersFlow (OrderId, StatusName,  Changed,  Changer,    DepartmentId, Refusual ) " +
                       " values                              (@OrderId, @StatusName,getdate(),    @UserId ,     @DepId,    @cause ); " +
                       " commit;";
            }
        }

        public string GET_EXEMPLAR_AVAILABILITY_STATUS
        {
            get
            {
                return  " select top 1 ID from Circulation..Orders where ExemplarId = @idData and Fund = @fund " +
                        " and StatusName in ('Заказ сформирован','Сотрудник хранения подбирает книгу','На бронеполке','Выдано в зал','Выдано на дом','Выдано с чужой бронеполки'," +
                        " 'Электронная выдача','Ожидает выдачи','Для возврата в хранение') ";
            }

        }

        public string IS_ISSUED_TO_READER
        {
            get
            {
                return " select top 1 ID from Circulation..Orders where ExemplarId = @idData and Fund = @fund " +
                        " and StatusName in ('Выдано в зал','Выдано на дом','Выдано с чужой бронеполки') ";
                        
            }
        }
    }
}
