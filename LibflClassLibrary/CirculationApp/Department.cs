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

        public Department() 
        {
            ExpectedBar = 0;
        }


        CirculationInfo ci = new CirculationInfo();
        public BJBookInfo ScannedBook;
        public BJExemplarInfo ScannedExemplar;
        public ReaderInfo ScannedReader;
        public BJUserInfo bjUser;
        /// <summary>
        /// Возвращаемые значения:
        /// 0 - Издание принято от читателя. Сдано/
        /// 1 - Штрихкод не найден ни в базе читателей ни в базе книг
        /// 2 - ожидался штрихкод читателя, а считан штрихкод издания
        /// 3 - ожидался штрихкод издания, а считан штрихкод читателя
        /// 4 - Издание подготовлено к выдаче. ожидаем штрихкод читателя
        /// 5 - Издание и читатель подготовлены к выдаче
        /// 
        /// </summary>
        /// <param name="PortData"></param>
        public int Circulate(string PortData, BJUserInfo bjUser)
        {
            this.bjUser = bjUser;
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

        //принять книгу от читателя
        public void RecieveBook(BJUserInfo bjUser)
        {

            //DBGeneral dbg = new DBGeneral();
            //dbg.Recieve(ScannedBook, ScannedReader, IDEMP);

        }

        private BARType BookOrReader(string data) //false - книга, true - читатель
        {
            return ci.CheckBAR(data);
        }


        //0 - успех
        //1 - нет или закончились права для выдачи на дом
        public void IssueBookToReader()
        {
            try
            {
                ci.IssueBookToReader(ScannedExemplar, ScannedReader);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            
            //проверить права абонемента на дом
            //if (ScannedReader.Rights[ReaderRightsEnum.FreeAbonement] != null)
            //{
            //    if (ScannedReader.Rights[ReaderRightsEnum.FreeAbonement].DateEndReaderRight < DateTime.Now)
            //    {
            //        return 1;
            //    }
            //    else
            //    {
            //        ci.IssueBookToReader(ScannedExemplar, ScannedReader);
            //        return 0;
            //    }
            //}
            //else
            //{
            //    return 1;
            //}


            //DBGeneral dbg = new DBGeneral();

            //if (ScannedBook.FUND == Bases.BJACC)
            //{
            //    if (CheckEmployeeRights())
            //    {
            //        dbg.ISSUE(ScannedBook, ScannedReader, IDEMP);
            //        return 0;
            //    }
            //    else
            //    {
            //        if (!CheckFreeAbonementRights())
            //        {
            //            return 1;
            //        }
            //        else
            //        {
            //            dbg.ISSUE(ScannedBook, ScannedReader, IDEMP);
            //            return 0;
            //        }
            //    }
            //}
            //else
            //{
            //    if ((ScannedReader.ReaderRights & Rights.EMPL) == Rights.EMPL)//если сотрудник выдаем сразу на дом
            //    {
            //        dbg.ISSUE(ScannedBook, ScannedReader, IDEMP);
            //    }
            //    else
            //    {
            //        if (ScannedBook.F899b == "ВХ")
            //        {
            //            if (!CheckFreeAbonementRights())
            //            {
            //                return 1;
            //            }
            //            dbg.ISSUE(ScannedBook, ScannedReader, IDEMP);
            //        }
            //        else
            //        {
            //            dbg.IssueInHall(ScannedBook, ScannedReader, IDEMP);

            //        }
            //    }

            //}
            //return 0;
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
        public void Prolong(int idiss, int days, int idemp)
        {
            //DBReader dbr = new DBReader();
            //dbr.ProlongByIDISS(idiss,days,idemp);

        }



        public void RemoveResponsibility(int idiss, int EmpID)
        {
           // DBGeneral dbg = new DBGeneral();
            //dbg.RemoveResposibility(idiss, EmpID);
            return;
        }

        //public int GetAttendance()
       // {
       //    // DBGeneral dbg = new DBGeneral();
       //     //return dbg.GetAttendance();
       // }

       // //public void AddAttendance(ReaderVO reader)
       // {
       //    // DBGeneral dbg = new DBGeneral();
       //    // dbg.AddAttendance(reader);
       // }

       //// public int GetCountOfPrologedTimes(int value)
       // {
       //     //DBGeneral dbg = new DBGeneral();
       //     //return dbg.GetCountOfPrologedTimes(value);
       // }
    }
}
