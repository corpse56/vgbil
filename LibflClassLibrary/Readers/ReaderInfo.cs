using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Readers
{

    public enum TypeReader { Local = 0, Remote = 1 };
    
    public class ReaderInfo
    {
        public ReaderInfo() {}
        public int NumberReader {get;set;}

      //,[NumberSC]
      //,[SerialSC]
        public string BarCode { get; set; }
      //,[Password]
        public string FamilyName { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
      //,[FamilyNameFind]
      //,[NameFind]
      //,[FatherNameFind]
        public DateTime DateBirth { get; set; }
      //,[Document]
      //,[DocumentNumber]
      //,[DateRegistration]
      //,[DateReRegistration]
        public string MobileTelephone { get; set; }
        public string Email { get; set; }
      //[WorkDepartment]
      //,[RegistrationCountry]
      //,[RegistrationRegion]
      //,[RegistrationProvince]
      //,[RegistrationDistrict]
      //,[RegistrationCity]
      //,[RegistrationStreet]
      //,[RegistrationHouse]
      //,[RegistrationFlat]
      //,[RegistrationTelephone]
      //,[LiveRegion]
      //,[LiveProvince]
      //,[LiveDistrict]
      //,[LiveCity]
      //,[LiveStreet]
      //,[LiveHouse]
      //,[LiveFlat]
      //,[LiveTelephone]
      //,[InfringerAll]
      //,[Infringer]
      //,[ClassInfringer]
      //,[InfringerDataEnd]
      //,[PenaltyID]
      //,[SheetWithoutCard]
      //,[SheetWithoutCardData]
      //,[SpecialNote]
      //,[EditorCreate]
      //,[EditorEnd]
      //,[EditEndDate]
      //,[EditorNow]
      //,[SelfRecord]
      //,[ReRegistration]
      //,[AbonementType]
      //,[IDOldAbonement]
      //,[InBlackList]
      //,[Tutor]
      //,[Child]
      //,[Photo]
      //,[Invalid]
      //,[WordReg]
      //,[InputAlwaysDate]
        public TypeReader TypeReader;

        public static ReaderInfo GetReader(int Id)
        {
            ReaderLoader loader = new ReaderLoader();
            ReaderInfo result = loader.LoadReader(Id);
            return result;
        }


        public bool IsFiveElBooksIssued()
        {
            ReaderLoader loader = new ReaderLoader();
            bool result = loader.IsFiveElBooksIssued(this.NumberReader);
            return result;
        }



    
    }


}
