﻿using LibflClassLibrary.Readers.Loaders;
using LibflClassLibrary.Readers.ReadersRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Readers.ReadersRight
{
    public class ReaderRightsInfo
    {
        public List<ReaderRight> RightsList = new List<ReaderRight>();
        private int NumberReader;

        public ReaderRight this[ReaderRightsEnum right]
        {
            get
            {
                return this.FindRight(right);
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var right in RightsList)
            {
                if (right.ReaderRightValue == ReaderRightsEnum.ReadingRoomUser) continue;
                result.Append($"{right.ToString()} до {right.DateEndReaderRight.ToString("dd.MM.yyyy")}; ");
            }
            return result.ToString();
        }

        private ReaderRight FindRight(ReaderRightsEnum right)
        {
            ReaderRight result = null;

            result = RightsList.Find(x => ((x.ReaderRightValue & right) == right));

            return result;
        }
        
        public static ReaderRightsInfo GetReaderRights(int NumberReader)
        {
            ReaderRightsInfo result = new ReaderRightsInfo();
            ReaderLoader loader = new ReaderLoader();
            result = loader.GetReaderRights(NumberReader);
            result.NumberReader = NumberReader;
            return result;

        }

        internal void GiveFreeAbonementRight()
        {
            ReaderLoader loader = new ReaderLoader();
            loader.GiveFreeAbonementRight(NumberReader);
        }
    }
}
