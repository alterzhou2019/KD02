using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace ZebraProject.Models.Zebra
{
    public class ZplLibrary
    {

        public PrinterOption PrinterOption { get; set; }

        public ZplLibrary(PrinterOption option)
        {
            this.PrinterOption = option;
        }

        // invoke this function to get ZPL by printer Option
        public List<string> GetZpl()
        {
            var zplList = new List<string>();
            // get template with user adjust value
            string zplTemplate = GetZPLTemplateByPrintOption(this.PrinterOption.ZPL);

            //remove ^FX to show hidden element
            if (PrinterOption.ShowHidden)
                zplTemplate = RemoveCommentOutTag(zplTemplate);
            zplList = GetZPLWithValue( zplTemplate);
            zplList = RemoveEmptyBarcode(zplList);
            return zplList;
        }

        public List<SelectListItemInline> GetHiddenOption()
        {
            var LabelScript = "";
            ZplScript script = new ZplScript(LabelScript);
            return script.GetHiddenOption();
        }

        private string RemoveCommentOutTag(string zpltext)
        {
            var sTag = "^FX";
            if (zpltext.Contains(sTag))
            {
                return zpltext.Replace(sTag, "");
            }

            return zpltext;
        }

        private string GetZPLTemplateByPrintOption(string template)
        {
            //List<string> hiddenOptions = this.PrinterOption.HiddenOptions.Where(o => o.Selected).Select(o => o.Value).ToList();
            ZplScript script = new ZplScript(template);
            //script.Hidden(hiddenOptions);
            script.SetLabelCopy(PrinterOption.Copy);
            script.MarginLabel(10, 20);
            //script.MarginLabel(this.PrinterOption.Margin.Left, this.PrinterOption.Margin.Top);
            return script.Zpl;
        }

        private List<string> GetZPLWithValue(string zplTemplate)
        {
            List<string> zplList = new List<string>();
            // set the value by model
            var i = 0;
            var model = new LabelFinishGood("1", this.PrinterOption.InfoLabel);
            string label = model.FillTemplateValue(zplTemplate);
            zplList.Add(label);
            return zplList;
        }
        private List<string> RemoveEmptyBarcode(List<string> zplList)
        {
            // remove empty barcode data.
            for (int i = 0; i < zplList.Count(); i++)
            {
                ZplScript zpl = new ZplScript(zplList[i]);
                zpl.RemoveEmptyBarcode();
                zplList[i] = zpl.Zpl;
            }
            return zplList;
        }
    }
}