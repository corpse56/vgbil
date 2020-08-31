using LibflClassLibrary.DigitizationQuene.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.DigitizationQuene
{
    public class DigitizationQueneManager
    {
        DigitizationQueneItemLoader loader = new DigitizationQueneItemLoader();
        public List<DigitizationQueneItemInfo> GetQuene()
        {
            return loader.GetQuene();
        }
        public List<DigitizationQueneItemInfo> GetLast400Completed()
        {
            return loader.GetCompleted();
        }
        public List<DigitizationQueneItemInfo> GetDeleted()
        {
            return loader.GetDeleted();
        }
        public void AddToQuene(string bookId, int readerId)
        {
            try
            {
                loader.AddToQuene(bookId, readerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
