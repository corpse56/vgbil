using LibflClassLibrary.Readers;
using LibflClassLibrary.Readers.ReadersJSONViewers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.ResponseObjects.Readers
{
    public class ReaderViewFactory
    {
        public static ReaderSimpleView GetReaderSimpleView(ReaderInfo reader) 
        {
            ReaderSimpleView result = new ReaderSimpleView();
            result.FamilyName = reader.FamilyName;
            result.Name = reader.Name;
            result.FatherName = reader.FatherName;
            result.ReaderId = reader.NumberReader;
            result.MobilePhone = reader.MobileTelephone;
            result.Email = reader.Email;
            result.IsRemoteReader = reader.IsRemoteReader;
            result.IsPasswordExists = !string.IsNullOrEmpty(reader.HashedPassword);
            return result;
        }
    }
}
