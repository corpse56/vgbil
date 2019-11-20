using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BookClasses
{
   /* public class Book
    {
        public string language_;
        public string author_;
        public string titleSrt_;
        public string authorSrt_;

        public int CompareTo(Book other)
        {
            int res = String.Compare(this.language_, other.language_); //fixed comparing order
            if (res != 0) return res;

            string a = this.author_ != "" ? this.authorSrt_ : this.titleSrt_;
            string b = other.author_ != "" ? other.authorSrt_ : other.titleSrt_;
            return String.Compare(a, b);
        }
    }*/
    public class AccessionNumber
    {
        private string accessionNum_;
        public string AccessionNum
        {
            get
            {
                return this.accessionNum_;
            }
            set
            {
                this.accessionNum_ = value;
            }
        
        }

        private string accessionLabel_;
        public string AccessionLabel
        {
            get
            {
                return this.accessionLabel_;
            }
            set
            {
                this.accessionLabel_ = value;
            }
        }
        private string iDDATA;
        public string IDDATA
        {
            get
            {
                return this.iDDATA;
            }
            set
            {
                this.iDDATA = value;
            }

        }
        private bool isWriteOff;
        public bool IsWriteOff
        {
            get
            {
                return this.isWriteOff;
            }
            set
            {
                this.isWriteOff = value;
            }

        }
        private string Fund_;
        public string Fund
        {
            get
            {
                return this.Fund_;
            }
            set
            {
                this.Fund_ = value;
            }

        }

        public int Price { get; set; }
        
        public string Currency { get; set; }





        public AccessionNumber(string num,string label,string idd,bool woff, string fund)
        { 
            this.accessionNum_ = num;
            this.accessionLabel_ = label;
            this.iDDATA = idd;
            this.isWriteOff = woff;
            this.Fund_ = fund;

        }
    }

    //==========================================================================================================
    //==========================================================================================================

    public class Book : IComparable<Book>
    {


        public string IDMAIN;
        private string iddataact;//язык
        public string IDDATAACT
        {
            get
            {
                return this.iddataact;
            }
            set
            {
                this.iddataact = value;
            }
        }
        private string language_;//язык
        public string Language
        {
            get
            {
                return this.language_;
            }
            set
            {
                this.language_ = value;
            }
        }
        private string author_;//автор
        public string Author
        {
            get
            {
                return this.author_;
            }
            set
            {
                this.author_ = value;
            }
        }
        private string authorSrt_;//автор для сортировки
        public string AuthorSrt
        {
            get
            {
                return this.authorSrt_;
            }
            set
            {
                this.authorSrt_ = value;
            }
        }

        private string title_;//заглавие
        public string Title
        {
            get
            {
                return this.title_;
            }
            set
            {
                this.title_ = value;
            }
        }

        private string titleSrt_;//занлавие для сортировки
        public string TitleSrt
        {
            get
            {
                return this.titleSrt_;
            }
            set
            {
                this.titleSrt_ = value;
            }
        }

        private string placeOfPublish_;//место издания
        public string PlaceOfPublish
        {
            get
            {
                return this.placeOfPublish_;
            }
            set
            {
                this.placeOfPublish_ = value;
            }
        }

        private string yearOfPublish_;//дата издания
        public string YearOfPublish
        {
            get
            {
                return this.yearOfPublish_;
            }
            set
            {
                this.yearOfPublish_= value;
            }
        }

        private string referenceNumberOF_;//расстановочный шифр осн. фонд
        public string ReferenceNumberOF
        {
            get
            {
                return this.referenceNumberOF_;
            }
            set
            {
                this.referenceNumberOF_ = value;
            }
        }
        private string referenceNumberAB_;//расстановочный шифр абонемент
        public string ReferenceNumberAB
        {
            get
            {
                return this.referenceNumberAB_;
            }
            set
            {
                this.referenceNumberAB_ = value;
            }
        }
        private string referenceNumberR_;//расстановочный шифр редкой книги
        public string ReferenceNumberR
        {
            get
            {
                return this.referenceNumberR_;
            }
            set
            {
                this.referenceNumberR_ = value;
            }
        }

        
        
        public List<AccessionNumber> accNums_;//номер+метка+иддата

        public void AddAccessionNum(AccessionNumber anIn)//ищем ИДДАТА и присваиваем инв.номер. если нет такого ИДДАТА - добавляем
        {
            bool wasIddata = false;
            if (this.accNums_ == null)
                this.accNums_ = new List<AccessionNumber>();
            foreach (AccessionNumber a in this.accNums_)
            {
                if (anIn.IDDATA.Equals(a.IDDATA))
                {
                    a.AccessionNum = anIn.AccessionNum;
                    wasIddata = true;
                }
            }
            if (!wasIddata)
            {
                this.accNums_.Add(anIn);
            }
        }
        public void AddAccessionLabel(AccessionNumber anIn)
        {
            bool wasIddata = false;
            if (this.accNums_ == null)
                this.accNums_ = new List<AccessionNumber>();
            foreach (AccessionNumber a in this.accNums_)
            {
                if (anIn.IDDATA.Equals(a.IDDATA))
                {
                    a.AccessionLabel = anIn.AccessionLabel;
                    wasIddata = true;
                }
            }
            if (!wasIddata)
            {
                this.accNums_.Add(anIn);
            }
        }
        public void AddPrice(AccessionNumber anIn)
        {
            bool wasIddata = false;
            if (this.accNums_ == null)
                this.accNums_ = new List<AccessionNumber>();
            foreach (AccessionNumber a in this.accNums_)
            {
                if (anIn.IDDATA.Equals(a.IDDATA))
                {
                    a.Price = anIn.Price;
                    wasIddata = true;
                }
            }
            if (!wasIddata)
            {
                this.accNums_.Add(anIn);
            }
        }
        public void AddCurrency(AccessionNumber anIn)
        {
            bool wasIddata = false;
            if (this.accNums_ == null)
                this.accNums_ = new List<AccessionNumber>();
            foreach (AccessionNumber a in this.accNums_)
            {
                if (anIn.IDDATA.Equals(a.IDDATA))
                {
                    a.Currency = anIn.Currency;
                    wasIddata = true;
                }
            }
            if (!wasIddata)
            {
                this.accNums_.Add(anIn);
            }
        }
        public void AddAccessionWriteOff(AccessionNumber anIn)
        {
            bool wasIddata = false;
            if (this.accNums_ == null)
                this.accNums_ = new List<AccessionNumber>();
            foreach (AccessionNumber a in this.accNums_)
            {
                if (anIn.IDDATA.Equals(a.IDDATA))
                {
                    a.IsWriteOff = true;
                    wasIddata = true;
                }
            }
            if (!wasIddata)
            {
                anIn.IsWriteOff = true;
                this.accNums_.Add(anIn);
            }
        }
        
        //public Book(string language, string author, string title,
        //            string authorSrt, string titleSrt,
        //            string placeOfPublish, string yearOfPublish,
        //            string referenceNumber 
        //            )
        //{
        //    this.language_ = language;
        //    this.author_ = author;
        //    this.title_ = title;
        //    this.authorSrt_ = authorSrt;
        //    this.titleSrt_ = titleSrt;
        //    this.placeOfPublish_ = placeOfPublish;
        //    this.yearOfPublish_ = yearOfPublish;
        //    this.referenceNumber_ = referenceNumber;
        //    //this.accessionLabel_ = accessionLabel;
        //    //this.accessionNumberOn_ = accessionNumberOn;
        //    //this.accessionNumberOff_ = accessionNumberOff;                       
        //}
        public Book()
        {
        }
        #region IComparable<Book> Members

        public int CompareTo(Book other) // первичное условие - язык. внутри языка сортируется по автору. если автора нет то объектом сортировки становиться заглавие. (вот такая вот фигня)
        {
            int res = String.Compare(this.language_, other.language_);
            if (res != 0)
                return res;

            if ((this.author_ != "") && (other.author_ != "")) //в обоих книгах есть автор
            {
                res = String.Compare(this.authorSrt_, other.authorSrt_);
                if (res != 0)
                    return res;
            }
            if ((this.author_ == "") && (other.author_ != "")) //во входном объекте книги автора нет, в текущем есть - сравниваем по заглавию входного объекта книги и автору текущего
            {
                res = String.Compare(this.titleSrt_, other.authorSrt_);
                if (res != 0)
                    return res;
            }
            if ((this.author_ != "") && (other.author_ == "")) //во входном объекте книги автор есть, в текущем нет - сравниваем по автору входного объекта книги и заглавию текущего
            {
                res = String.Compare(this.authorSrt_, other.titleSrt_);
                if (res != 0)
                    return res;
            }
            //if ((other.author_ == "") && (this.author_ == "")) // авторов нет - сортируем по заглавию
            //{
            return res = String.Compare(this.titleSrt_, other.titleSrt_);
                //if (res != 0)
                //    return res;
            //}

        }

        #endregion
        public int InvsLeftInFund
        {
            get
            {
                int result = 0;
                foreach (AccessionNumber n in this.accNums_)
                {
                    if (n.IsWriteOff)
                        result++;
                }
                return this.accNums_.Count - result;
            }
        }
        public static string CountAllWOFFInvs(List<Book> b_)
        {
                int result = 0;
                foreach (Book b in b_)
                {
                    foreach (AccessionNumber n in b.accNums_)
                    {
                        if (n.IsWriteOff)
                            result++;
                    }
                }
                return result.ToString();
        }

        internal void AddPrice(string idData, System.Data.DataTable dataTable)
        {
            //всё по-уродски, но переделывать было лень. поэтому просто под существующий код подстроился
            //ищем по таблице поле цена и потом ищем инвентарь, к которому она относится.
            bool f922c  = false;
            bool f922d = false;
            foreach (DataRow row in dataTable.Rows)
            {
                if ((row["IDDATA"].ToString() == idData) && (row["MNFIELD"].ToString() + row["MSFIELD"].ToString() == "922$c"))
                {
                    foreach (AccessionNumber inv in this.accNums_)
                    {
                        if (inv.IDDATA == idData)
                        {
                            f922c = true;
                            int Price = -1;
                            if (int.TryParse(row["SORT"].ToString(), out Price))
                            {
                                inv.Price = Price / 100;
                            }
                            else
                            {
                                inv.Price = -1;//-1 невозможно распарсить цифру. -2 нет поля
                            }
                        }
                    }
                }
                if ((row["IDDATA"].ToString() == idData) && (row["MNFIELD"].ToString() + row["MSFIELD"].ToString() == "922$d"))
                {
                    foreach (AccessionNumber inv in this.accNums_)
                    {
                        if (inv.IDDATA == idData)
                        {
                            f922d = true;
                            if (row["PLAIN"].ToString() == string.Empty)
                            {
                                inv.Currency = "<пустое значение>";
                            }
                            else
                            {
                                inv.Currency = row["PLAIN"].ToString();
                            }
                            
                           
                        }
                    }
                }

            }
            if (!f922c)
            {
                foreach (AccessionNumber inv in this.accNums_)
                {
                    if (inv.IDDATA == idData)
                    {
                        inv.Price = -2;//-1 невозможно распарсить цифру. -2 нет поля
                    }
                }
            }
            if (!f922d)
            {
                foreach (AccessionNumber inv in this.accNums_)
                {
                    if (inv.IDDATA == idData)
                    {                        
                        inv.Currency = "<нет поля>";
                    }
                }
            }

        }
    }
    class Books
    {
        private List<Book> books_;

        public Books()
        {
            books_ = new List<Book>();
        }
        /*List<Book> BookByLanguage()
        {
        }*/
    }


}
