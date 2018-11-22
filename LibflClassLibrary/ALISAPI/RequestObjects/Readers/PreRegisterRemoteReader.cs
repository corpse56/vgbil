using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.RequestObjects.Readers
{
    public class PreRegisterRemoteReader
    {
        public string FamilyName { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; }
        public string MobilePhone { get; set; }
        public string Password { get; set; }
    }
}
