using ALISAPI.Errors;
using LibflClassLibrary.ALISAPI.Errors;
using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.Readers;
using LibflClassLibrary.Readers.ReadersRight;
using LibflClassLibrary.Readers.ReadersRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CirculationApp
{
    public enum BARType { Book, Reader, NotExist }
    public enum ExpectingAction { WaitingBook, WaitingReader, WaitingConfimation }//0 - ожидается штрихкод книги, 1 - ожидается штрихкод читателя, 2 - ожидается подтверждение или отмена выдачи

    public class Department 
    {
        public ExpectingAction ExpectedBar = ExpectingAction.WaitingBook;

        public Department(BJUserInfo bjUser) 
        {
            ExpectedBar = 0;
            this.bjUser = bjUser;
        }


        CirculationInfo ci = new CirculationInfo();
        public BJBookInfo ScannedBook;
        public BJExemplarInfo ScannedExemplar;
        public ReaderInfo ScannedReader;
        public BJUserInfo bjUser;
        /// <summary>
        /// Возвращаемые значения:
        /// 0 - Издание принято от читателя. Сдано.
        /// 1 - Штрихкод не найден ни в базе читателей ни в базе книг
        /// 2 - ожидался штрихкод читателя, а считан штрихкод издания
        /// 3 - ожидался штрихкод издания, а считан штрихкод читателя
        /// 4 - Издание подготовлено к выдаче. ожидаем штрихкод читателя
        /// 5 - Издание и читатель подготовлены к выдаче
        /// 
        /// </summary>
        /// <param name="PortData"></param>
        public int Circulate(string PortData)
        {
            BARType ScannedType;
            if (ExpectedBar == ExpectingAction.WaitingConfimation)//если ожидается подтверждение выдачи
            {
                return 5;
            }

            if ((ScannedType = BookOrReader(PortData)) == BARType.NotExist)//существует ли такой штрихкод вообще либо в базе читателей либо в базе изданий
            {
                return 1;
            }
            
            if (ExpectedBar == ExpectingAction.WaitingBook)//если сейчас ожидается штрихкод книги
            {
                if (ScannedType == BARType.Reader) //выяснить какой штрихкод сейчас считан: читатель или книга
                {
                    return 3;
                }
                this.ScannedBook = BJBookInfo.GetBookInfoByBAR(PortData);
                ScannedExemplar = (BJExemplarInfo)ScannedBook.Exemplars.Find(x => ((BJExemplarInfo)x).Bar == PortData);
                if (ci.IsIssuedToReader(ScannedExemplar))
                {
                    return 0;
                }
                else
                {
                    ExpectedBar = ExpectingAction.WaitingReader;
                    return 4;
                }
            }
            else  //если сейчас ожидается штрихкод читателя
            {
                if (ScannedType == BARType.Book) //выяснить какой штрихкод сейчас считан: читатель или книга
                {
                    return 2;
                }
                ScannedReader = ReaderInfo.GetReaderByBar(PortData);// new ReaderVO(PortData);
                ExpectedBar = ExpectingAction.WaitingConfimation;
                return 5;
                
            }
        }


        private BARType BookOrReader(string data) //false - книга, true - читатель
        {
            return ci.CheckBAR(data);
        }


        public void IssueBookToReader()
        {
            try
            {
                ci.IssueBookToReader(ScannedExemplar, ScannedReader, bjUser);
            }
            catch (Exception e)
            {
                ALISError error = ALISErrorList._list.Find(x => x.Code == e.Message);
                MessageBox.Show(error.Message);
                return;
            }
            
        }

        public void AttendanceScan(string barcode)
        {
            ci.AttendanceScan(barcode, bjUser);
        }

        private bool CheckFreeAbonementRights()
        {
            ReaderRightsInfo rights = ReaderRightsInfo.GetReaderRights(ScannedReader.NumberReader);
            if (rights[ReaderRightsEnum.FreeAbonement] == null)
            {
                return false;
            }
            return true;
        }
        private bool CheckEmployeeRights()
        {
            ReaderRightsInfo rights = ReaderRightsInfo.GetReaderRights(ScannedReader.NumberReader);
            if (rights[ReaderRightsEnum.Employee] == null)
            {
                return false;
            }
            return true;
        }

        public int GetAttendance()
        {
            return ci.GetAttendance(bjUser);
        }


        public void RemoveResponsibility(int idiss, int EmpID)
        {
           // DBGeneral dbg = new DBGeneral();
            //dbg.RemoveResposibility(idiss, EmpID);
            return;
        }

        public void RecieveBook(string fromPort)
        {
            BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByBar(fromPort);
            OrderInfo oi = ci.FindOrderByExemplar(exemplar);

            if (ci.RecieveBookFromReader(exemplar, oi, bjUser) == 1)
            {
                DialogResult dr = MessageBox.Show("Читатель сдаёт книгу на бронеполку?", "Внимание!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    ci.ChangeOrderStatusReturn(bjUser, oi.OrderId, CirculationStatuses.InReserve.Value); 
                }
                else if (dr == DialogResult.No)
                {
                    ci.ChangeOrderStatusReturn(bjUser, oi.OrderId, CirculationStatuses.ForReturnToBookStorage.Value); 
                }

            }
        }

        public int GetIssuedInHallBooksCount()
        {
            if (bjUser == null) return 0;
            return ci.GetIssuedInHallBooksCount(bjUser);
        }

        //// public int GetCountOfPrologedTimes(int value)
        // {
        //     //DBGeneral dbg = new DBGeneral();
        //     //return dbg.GetCountOfPrologedTimes(value);
        // }
    }
}
