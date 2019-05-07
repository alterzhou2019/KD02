using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ZebraProject.Models.Zebra
{
    public class ZplModel
    {
        public void Init ()
        {
        }
        public void GetPreviewOption()
        {
            //get the printing option
            var printerOption = new PrinterOption("");
            printerOption.IPAddress = "172.21.168.24";//打印机IP
            printerOption.ZPL = " ^XA ^CW1, E:Calibri.FNT  ^FO100,50,^A1N,50,50 " +
"^FD[var_fixed_assets_Id]"
+ " ^FD[RAM] "
+"^FD[HDD]"
+"^FD[CPU] "+""
+"^FD[PrintTime] "
+"^FD[Inputtime]" 
+"^XZ";
            printerOption.Copy = 1;//打印数量
            var infolabel = new InfoLabel();
            infolabel.var_fixed_assets_Id = "Q-19567";
            infolabel.RAM = "8G";
            infolabel.HDD = "500G";
            infolabel.CPU = "I5-2300";
            infolabel.PrintTime = "2019-5-6";
            infolabel.Inputtime = "2011-4-2";
            printerOption.InfoLabel = infolabel;//要传的参数
            try
            {
                var zebraPrinter = new ZebraPrinter(printerOption.IPAddress, 9100);
                zebraPrinter.Open();
                var zplLibrary = new ZplLibrary(printerOption);
                List<string> zplList = zplLibrary.GetZpl();
                foreach (var strZpl in zplList)
                {
                    zebraPrinter.Print(strZpl);
                }

                zebraPrinter.Close();
            }
            catch (Exception ex)
            {
                throw new HttpException(608, ex.Message);
            }
        }
    }
}