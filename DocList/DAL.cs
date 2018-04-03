using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocList
{
    class DAL
    {
        dbConnect db;
        BookEntity be;
        public DAL()
        {
            db = new dbConnect();
            be = new BookEntity(db.GeteConnectionString());
            //be = new BookEntity();
        }
        public BooksView GetBookByBar(string bar)
        {
            var result = (from b in be.BooksView
                          where b.bar == bar
                          select b).Take(1);
            if (result.Count() == 0)
                return null;
            return result.First();
        }
        public int ISBJVVV(string bar)
        {
            return db.ISBJVVV(bar);
        }



        internal BooksViewRED GetBookByBarRED(string bar)
        {
            var result = (from b in be.BooksViewRED
                          where b.bar == bar
                          select b).Take(1);
            if (result.Count() == 0)
                return null;
            return result.First();
        }
        internal BooksViewFCC GetBookByBarFCC(string bar)
        {
            var result = (from b in be.BooksViewFCC
                          where b.bar == bar
                          select b).Take(1);
            if (result.Count() == 0)
                return null;
            return result.First();
        }
    }
}
