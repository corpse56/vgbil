using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Circulation
{
    public enum BARTYPE { BookACC, Reader, NotExist, BookBJVVV }
    public enum ExpectingAction { WaitingBook, WaitingReader, WaitingConfimation }//0 - ожидается штрихкод книги, 1 - ожидается штрихкод читателя, 2 - ожидается подтверждение или отмена выдачи

    public class Department : DB
    {
        public ExpectingAction ExpectedBar = ExpectingAction.WaitingBook;

        public Department() 
        {
            ExpectedBar = 0;
        }
        


        public BookVO ScannedBook;
        public ReaderVO ScannedReader;
        /// <summary>
        /// Возвращаемые значения:
        /// 0 - Издание принято от читателя
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
            BARTYPE ScannedType;
            if (ExpectedBar == ExpectingAction.WaitingConfimation)//если ожидается подтверждение выдачи
            {
                return 5;
            }

            if ((ScannedType = BookOrReader(PortData)) == BARTYPE.NotExist)//существует ли такой штрихкод вообще либо в базе читателей либо в базе изданий
            {
                return 1;
            }
            
            if (ExpectedBar == 0)//если сейчас ожидается штрихкод книги
            {
                if (ScannedType == BARTYPE.Reader) //выяснить какой штрихкод сейчас считан: читатель или книга
                {
                    return 3;
                }
                this.ScannedBook = new BookVO(PortData);
                if (ScannedBook.IsIssued())
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
                if ((ScannedType == BARTYPE.BookACC) || (ScannedType == BARTYPE.BookBJVVV)) //выяснить какой штрихкод сейчас считан: читатель или книга
                {
                    return 2;
                }
                ScannedReader = new ReaderVO(PortData);
                ExpectedBar = ExpectingAction.WaitingConfimation;
                return 5;
                
            }
        }

        public void RecieveBook(int IDEMP)
        {
            DBGeneral dbg = new DBGeneral();
            dbg.Recieve(ScannedBook, ScannedReader, IDEMP);
        }

        private BARTYPE BookOrReader(string data) //false - книга, true - читатель
        {
            var dbg = new DBGeneral();
            
            return dbg.BookOrReader(data);
        }

        internal int ISSUE(int IDEMP)
        {
            DBGeneral dbg = new DBGeneral();
            return dbg.ISSUE(ScannedBook, ScannedReader, IDEMP);

        }

        internal void Prolong(int idiss, int days,int idemp)
        {
            DBReader dbr = new DBReader();
            dbr.ProlongByIDISS(idiss,days,idemp);

        }



        internal void RemoveResponsibility(int idiss, int EmpID)
        {
            DBGeneral dbg = new DBGeneral();
            dbg.RemoveResposibility(idiss, EmpID);
            return;
        }

        internal int GetAttendance()
        {
            DBGeneral dbg = new DBGeneral();
            return dbg.GetAttendance();
        }

        internal void AddAttendance(ReaderVO reader)
        {
            DBGeneral dbg = new DBGeneral();
            dbg.AddAttendance(reader);
        }


    }
}
