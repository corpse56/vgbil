using ALISAPI.Errors;
using LibflClassLibrary.ALISAPI.Errors;
using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.Circulation.CirculationService;
using LibflClassLibrary.ImageCatalog;
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
    public enum ExpectingActionForAssignCardToCatalog { WaitingCardBar, WaitingBookBar, WaitingConfirmation }
    public class Department 
    {
        public ExpectingAction ExpectedBar = ExpectingAction.WaitingBook;
        public ExpectingActionForAssignCardToCatalog ExpectedActionForCard = ExpectingActionForAssignCardToCatalog.WaitingCardBar;

        public Department(BJUserInfo bjUser) 
        {
            ExpectedBar = 0;
            this.bjUser = bjUser;
        }


        CirculationInfo ci = new CirculationInfo();
        ImageCatalogCirculationManager icCirc = new ImageCatalogCirculationManager();
        public ExemplarBase ScannedExemplar;
        public ReaderInfo ScannedReader;
        public BJUserInfo bjUser;

        //public string scannedCardRequirmentBar;
        //public string scannedBookBar;
        //public int scannedICOrderId;
        public ICOrderInfo scannedICOrder;
        public ExemplarBase scannedICExemplar;
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
                ScannedExemplar = ExemplarFactory.CreateExemplar(PortData);
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

        public void RecieveImCatOrderToCafedra(string fromport)
        {
            if (fromport.Substring(0, 1) != "M")
            {
                throw new Exception("Считанный штрихкод не является штрихкодом с требования карточного каталога");
            }
            int icOrderId = 0;
            string tmp = fromport.Substring(1, fromport.Length - 1);
            try
            {
                icOrderId = Convert.ToInt32(tmp);
            }
            catch
            {
                throw new Exception("Ошибка при декодировании штрихкода в номер заказа");
            }

            ICOrderInfo iCOrder = ICOrderInfo.GetICOrderById(icOrderId, false);

            if (iCOrder == null) throw new Exception($"Ошибка при получении заказа из базы. Номер заказа {icOrderId}");

            ImageCatalogCirculationManager cm = new ImageCatalogCirculationManager();
            cm.ChangeOrderStatus(iCOrder, bjUser, CirculationStatuses.WaitingFirstIssue.Value);


        }

        public int GetAttendance()
        {
            return ci.GetAttendance(bjUser);
        }

        public void RecieveBook(string fromPort)
        {
            ExemplarBase exemplar = ExemplarFactory.CreateExemplar(fromPort);
            OrderInfo oi = ci.FindOrderByExemplar(exemplar);
            if (exemplar.circulation.exemplarRecieverFromReader.IsNeedToAskReaderForReserve(exemplar, oi))
            {
                DialogResult dr = MessageBox.Show("Читатель сдаёт книгу на бронеполку?", "Внимание!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    exemplar.circulation.exemplarRecieverFromReader.RecieveBookFromReader(exemplar, oi, bjUser, CirculationStatuses.InReserve.Value);
                }
                else if (dr == DialogResult.No)
                {
                    exemplar.circulation.exemplarRecieverFromReader.RecieveBookFromReader(exemplar, oi, bjUser, CirculationStatuses.ForReturnToBookStorage.Value);
                }
            }
            else
            {
                exemplar.circulation.exemplarRecieverFromReader.RecieveBookFromReader(exemplar, oi, bjUser);
            }
        }

        public void AssignCardToCatalog(string fromport)
        {
            if (ExpectedActionForCard == ExpectingActionForAssignCardToCatalog.WaitingCardBar)
            {
                if (fromport.Substring(0,1) != "M")
                {
                    throw new Exception("Считанный штрихкод не является штрихкодом с требования карточного каталога");
                }
                int icOrderId = 0;
                string tmp = fromport.Substring(1, fromport.Length-1);
                try
                {
                    icOrderId = Convert.ToInt32(tmp);
                }
                catch 
                {
                    throw new Exception("Ошибка при декодировании штрихкода в номер заказа");
                }
                
                ICOrderInfo iCOrder = ICOrderInfo.GetICOrderById(icOrderId, false);

                scannedICOrder = iCOrder ?? throw new Exception($"Ошибка при получении заказа из базы. Номер заказа {icOrderId}");

                this.ExpectedActionForCard = ExpectingActionForAssignCardToCatalog.WaitingBookBar;
                return;
            }
            if (ExpectedActionForCard == ExpectingActionForAssignCardToCatalog.WaitingBookBar)
            {
                if ((fromport.Substring(0, 1) != "U") && (fromport.Substring(0, 1) != "Y"))
                {
                    throw new Exception("Считанный штрихкод не является штрихкодом книги либо периодики");
                }
                ExemplarBase exemplar = ExemplarFactory.CreateExemplar(fromport);
                scannedICExemplar = exemplar ?? throw new Exception($"Книга со штрихкодом {fromport} не найдена.");
                this.ExpectedActionForCard = ExpectingActionForAssignCardToCatalog.WaitingConfirmation;
                return;
            }
        }

        public int GetIssuedInHallBooksCount()
        {
            if (bjUser == null) return 0;
            return ci.GetIssuedInHallBooksCount(bjUser);
        }
    }
}
