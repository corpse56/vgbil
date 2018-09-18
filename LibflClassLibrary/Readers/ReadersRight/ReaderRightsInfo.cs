using LibflClassLibrary.Readers.Loaders;
using LibflClassLibrary.Readers.ReadersRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Readers.ReadersRight
{
    public class ReaderRightsInfo
    {
        public List<ReaderRight> Rights = new List<ReaderRight>();
        public ReaderRight this[ReaderRightsEnum right]
        {
            get
            {
                return this.FindRight(right);
            }
        }

        private ReaderRight FindRight(ReaderRightsEnum right)
        {
            ReaderRight result = null;

            result = Rights.Find(x => ((x.ReaderRightValue & right) == right));

            return result;
        }
        
        public static ReaderRightsInfo GetReaderRights(int NumberReader)
        {
            ReaderRightsInfo result = null;
            ReaderLoader loader = new ReaderLoader();
            result = loader.GetReaderRights(NumberReader);
            return result;

        }
    }
}
