using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocList
{
    class BookBLL
    {
        private DAL dal;
        public BookBLL()
        {
            dal = new DAL();
        }
        public BooksView GetBookByBar(string bar)
        {
            return dal.GetBookByBar(bar);
        }
        public BooksViewRED GetBookByBarRED(string bar)
        {
            return dal.GetBookByBarRED(bar);
        }
        public BooksViewFCC GetBookByBarFCC(string bar)
        {
            return dal.GetBookByBarFCC(bar);
        }
        public int ISBJVVV(string bar)
        {
            return this.dal.ISBJVVV(bar);
        }

    }
}
