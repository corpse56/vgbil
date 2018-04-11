﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security;

namespace ExportBJ_XML.classes
{

    public abstract class VuFindConverter
    {
        public string Fund
        { 
            get 
            {
                return this._fund; 
            }
            protected set
            {
                this._fund = value;
            }
        }
        private string _fund = "unknown";

        public event EventHandler RecordExported;
        //public event EventHandler<VuFindConverterEventArgs> OnDatabaseTimeout;
        //event EventHandler<VuFindConverterEventArgs> OnConvertError;

        public VuFindConverter() 
        {

            


        }

        public abstract void Export();
        public abstract void ExportSingleRecord(int idRecord);
        public abstract void ExportCovers();

        protected virtual void OnRecordExported(EventArgs e)
        {
            EventHandler handler = RecordExported;
            if (handler != null)
            {
                handler(this, e);
            }
        }

       

        public static string GetFundId(string fund)
        {

            switch (fund)
            {
                case "BJVVV":
                    return "5000";
                case "REDKOSTJ":
                    return "5001";
                case "BJACC":
                    return "5003";
                case "BJFCC":
                    return "5004";
                case "BJSCC":
                    return "5005";
                case "BRIT_SOVET":
                    return "5002";
                case "litres":
                    return "5007";
                case "period":
                    return "5006";
                case "HJB":
                    return "5009";
                case "pearson":
                    return "5008";
            }
            return "<неизвестный фонд>";
        }


    }
}
