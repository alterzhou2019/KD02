
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZebraProject.Models.Zebra
{
    public class LabelFinishGood:LabelModel
    {
       public string var_fixed_assets_Id { get; set; }
       public string CPU { get; set; }
        public string RAM { get; set; }
        public string HDD { get; set; }
        public string Inputtime { get; set; }
        public string PrintTime { get; set; }
        

            

        public LabelFinishGood(string sn, InfoLabel label)
        {
            this.var_fixed_assets_Id = label.var_fixed_assets_Id;
            this.RAM = label.RAM;
            this.HDD = label.HDD;
            this.CPU = label.CPU;
            this.PrintTime = label.PrintTime;
            this.Inputtime = label.Inputtime;

        }

    }
}