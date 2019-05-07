using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZebraProject.Models.Zebra
{
    public partial class PrinterOption
    {
        public InfoLabel InfoLabel { get; set; }
        //public InfoCarton InfoCarton { get; set; }

        private void InitExtension()
        {
            this.InfoLabel = new InfoLabel();
        }
    }


    public class InfoLabel
    {
        public string var_fixed_assets_Id { get; set; }
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string HDD { get; set; }
        public string Inputtime { get; set; }
        public string PrintTime { get; set; }


    }

}