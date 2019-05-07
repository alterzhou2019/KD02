
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ZebraProject.Models.Zebra
{
    /// <summary>
    /// This is standard class, if we extend new model just implement it in PrinterOptionExtension.cs
    /// </summary>
    public partial class PrinterOption
    {
        public string IPAddress { get; set; }

        public string Name { get; set; }
        public string ZPL { get; set; }

        public int Copy { get; set; }

        public Margin Margin { get; set; }

        public bool ShowHidden { get; set; }

        public InfoPrintPage InfoPrintPage { get; set; }

        // label template list
        public string LabelTemplateID { get; set; }

        public List<SelectListItem> LabelTemplateList { get; set; }

        // select List items
        public List<SelectListItemInline> HiddenOptions { get; set; }

        public List<SelectListItem> PrinterList { get; set; }

        public List<string> PreviewImages { get; set; }

        public PrinterOption()
        {
            Init();
            //this.LabelTemplateList = GetTemplateList();
        }

        public PrinterOption(string id)
        {
            //this.LabelTemplateList = GetTemplateList();
        }

        private void Init()
        {
        }

    }

    public class InfoPrintPage
    {
        public string PrintURL { get; set; }

        public string PreviewURL { get; set; }

        public string PrinterLocalStorageName { get; set; }

        public string TemaplteLocalStorageName { get; set; }

    }

    public class Margin
    {
        public double Left { get; set; }

        public double Right { get; set; }

        public double Top { get; set; }

        public double Bottom { get; set; }
    }

}