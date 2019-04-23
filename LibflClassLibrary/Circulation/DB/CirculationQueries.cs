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
                        " left join Circulation..OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName" +
                        " where A.StatusName in ('Заказ сформирован')";
            }
        }
        internal string GET_ORDERS_FOR_STORAGE_BY_STATUS
        {
            get
            {
                return " select A.*, B.Refusual " +
                        " from Circulation..Orders A " +
                        " left join Circulation..OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName" +
                        " where A.StatusName in (@statusName)";
            }
        }

        internal string GET_ORDERS_HISTORY_FOR_STORAGE
        {
            get
            {
                return " select top 1000 A.*,B.Refusual from Circulation..Orders A " +
                       " left join Circulation..OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName " +
                       " where A.StatusName not in ('Заказ сформирован', 'Электронная выдача' ) " +
                       " and A.StartDate >= dateadd(day, -10, getdate())  " +
                       " order by A.Id desc";
            }
        }

        internal string GET_ORDER
        {
            get
            {
                return " select A.*,B.Refusual from Circulation..Orders A " +
                        " left join Circulation..OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName" +
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
                       " left join Circulation..OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName" +
                       " where ReaderId = @ReaderId and A.StatusName not in ('Завершено', 'Для возврата в хранение')";
            }
        }

        public string GET_ORDERS_BY_STATUS
        {
            get
            {
                return " select A.*,B.Refusual from Circulation..Orders A " +
                       " left join Circulation..OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName" +
                       " where A.StatusName in (@circulationStatus)";
                       
            }
        }
        public string GET_ORDERS_BY_STATUS_AND_DEP
        {
            get
            {
                return " select A.* from Circulation..Orders A " +
                       " where A.StatusName in (@statusName) and IssueDepId = @depId";

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
        public string CHANGE_ORDER_STATUS_ISSUE
        {
            get
            {
                return " BEGIN TRANSACTION; " +
                       " insert into Circulation..OrdersFlow (OrderId, StatusName, Changed,  Changer,    DepartmentId, Refusual ) " +
                       " values                            (@OrderId, @StatusName,getdate(), @Changer, @DepartmentId, @Refusual );" +
                       " update Circulation..Orders set StatusName = @StatusName, IssueDate = getdate(), IssueDepId = @DepartmentId,  ReturnDate = DATEADD(day , @ReturnInDays , getdate()) " +
                       " where ID = @OrderId;" +
                       " COMMIT; ";
            }
        }
        public string CHANGE_ORDER_STATUS_RETURN
        {
            get
            {
                return " BEGIN TRANSACTION; " +
                       " insert into Circulation..OrdersFlow (OrderId, StatusName, Changed,  Changer,    DepartmentId, Refusual  ) " +
                       " values                            (@OrderId, @StatusName,getdate(), @Changer, @DepartmentId, @Refusual  );" +
                       " update Circulation..Orders set StatusName = @StatusName, FactReturnDate = getdate(), ReturnDepId = @DepartmentId where ID = @OrderId;" +
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

        public string REFUSE_ORDER//этот запрос сам вставляет действие оператора. Остальные используют метод ChangeStatus. не знаю зачем. можно переделать для однообразия.
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

        public string FIND_ORDER_BY_EXEMPLAR
        {
            get
            {
                return  " select * from Circulation..Orders A" +
                        " left join Circulation..OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName" +
                        " where ExemplarId = @idData and Fund = @fund " +
                        " and A.StatusName not in ('Завершено') ";

            }
        }

        public string NEW_ORDER_ISSUE_BOOK
        {
            get
            {
                return "insert into Circulation..Orders ( BookId,  ExemplarId,  ReaderId,  StatusName,    StartDate,        ReturnDate,                        Barcode,   Fund,      IssuingDepId, IssueDepId, IssueDate)" +
                       " values                         (@BookId,  @ExemplarId, @ReaderId, @StatusName,   getdate(), DATEADD(day , @ReturnInDays , getdate()), @Barcode, @Fund,    @IssuingDepId,  @IssueDepId, getdate() ); " +
                       " select SCOPE_IDENTITY() ";
            }
        }

        public string GET_ORDERS_FLOW
        {
            get
            {
                return  " select * from Circulation..OrdersFlow F " +
                        " left join Circulation..Orders O on F.OrderId = O.ID " +
                        " where DepartmentId = @depId and cast(cast(Changed as varchar(11)) as datetime) = cast(cast(GETDATE() as varchar(11)) as datetime) and O.ExemplarId != 0 " +
                        " order by Changed desc" ;
            }
        }
        public string GET_ORDERS_FLOW_BY_ORDER_ID
        {
            get
            {
                return " select * from Circulation..OrdersFlow  " +
                        " where OrderId  = @orderId" +
                        " order by Changed asc";
            }
        }

        public string IS_ALREADY_VISITED_TODAY
        {
            get
            {
                return  " select * from Circulation..Attendance where Barcode = @barcode and DepId = @depId " +
                        " and cast(cast(AttendanceDate as varchar(11)) as datetime) = cast(cast(getdate() as varchar(11)) as datetime)";
            }
        }

        public string ADD_ATTENDANCE
        {
            get
            {
                return  " insert into Circulation..Attendance (ReaderId, EmployeeId, DepId, AttendanceDate, Barcode) " +
                        " values                              (@readerId, @empId,    @depId, getdate(),     @barcode)";
            }
        }

        public string GET_TODAY_ATTENDANCE_BY_DEP
        {
            get
            {
                return  "select * from Circulation..Attendance where cast(cast(AttendanceDate as varchar(11)) as datetime) = cast(cast(getdate() as varchar(11)) as datetime) " +
                        " and DepId = @depId";
            }
        }

        public string GET_ORDERS_BY_EXEMPLAR
        {
            get
            {
                return  " select * from Circulation..Orders A " +
                        " left join Circulation..OrdersFlow B on A.ID = B.OrderId and B.StatusName = @RefusualStatusName" +
                        " where ExemplarId = @idData and fund =@fund ";
            }
        }

    }
}
