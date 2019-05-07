using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZebraProject.Models.Zebra;

namespace ZebraProject.Models.Zebra
{
    public class ZplScript
    {
        public string Zpl { get; set; }
        public ZplScript(string zpl)
        {
            this.Zpl = zpl;
            // avoid ZPL is empty.
            if (string.IsNullOrWhiteSpace(zpl))
            {
                this.Zpl = "";
            }
        }

        public void MarginLabel(double MarginLeft, double MarginTop)
        {
            string PositionElmenetName = "^FO";

            string[] foList = Zpl.Split(new string[] { PositionElmenetName }, StringSplitOptions.None);
            // foList > 1 it contain ^FO
            for (int i = 1; i < foList.Count(); i++)
            {
                try
                {
                    // ex: 50,50^BC^FD^FS 
                    // it will be splited to:
                    // [0]=> 50,50 
                    // [1]=> ^BC^FD^FS 
                    string[] foAndOtherElements = foList[i].Split(new string[] { "^" }, StringSplitOptions.None);

                    // split [0] to get every parameter
                    string[] foParameters = foAndOtherElements[0].Split(new string[] { "," }, StringSplitOptions.None);
                    foParameters[0] = (Convert.ToInt32(foParameters[0]) + MarginLeft).ToString();
                    foParameters[1] = (Convert.ToInt32(foParameters[1]) + MarginTop).ToString();
                    foAndOtherElements[0] = string.Join(",", foParameters);

                    foList[i] = string.Join("^", foAndOtherElements);

                }
                catch
                {
                    throw new HttpException(608, string.Format("Margin ^FO{0} is error", foList[i]));
                }
            }

            Zpl = string.Join(PositionElmenetName, foList);
        }

        public void SetLabelCopy(int copy)
        {
            if (Zpl.Contains("^PQ"))
            {
            }
            string[] labels = this.Zpl.Split(new string[] { "^XZ" }, StringSplitOptions.None);
            Zpl = string.Join(string.Format("^PQ{0}^XZ", copy), labels);
        }

        public void Hidden(List<string> hiddenOptions)
        {
            foreach (string option in hiddenOptions)
            {
                Hidden(option);
            }
        }

        private void Hidden(string hiddenOption)
        {
            // it clear but maybe too hard to understand.
            //string newZpl = Regex.Replace(zpl, string.Format(@"(\^FX(\s|\n)*?{0})((.|\n)*?)(\^FX(\s|\n)*?{0})", optionName), string.Empty); 

            // get all comment fields.
            string[] comments = Zpl.Split(new string[] { "^FX" }, StringSplitOptions.None);

            // hidden the comment section
            bool isHidden = false;
            for (int i = 1; i < comments.Count(); i++)
            {
                // check the comment section key.
                string hiddenKey = comments[i].Split(new string[] { "^" }, StringSplitOptions.None)[0];
                // turn ON or OFF hide the content
                if (hiddenKey.Trim() == hiddenOption.Trim())
                {
                    isHidden = !isHidden;
                }
                if (isHidden)
                {
                    comments[i] = "";
                }
            }

            // it means doesn't has the end hidden section.
            if (isHidden)
            {
               
            }

            Zpl = string.Join("^FX", comments);
        }

        public List<SelectListItemInline> GetHiddenOption()
        {
            List<SelectListItemInline> hiddenOptions = new List<SelectListItemInline>();

            if (!string.IsNullOrWhiteSpace(Zpl))
            {
                string[] comments = Zpl.Split(new string[] { "^FX" }, StringSplitOptions.None);
                for (int i = 1; i < comments.Count(); i++)
                {
                    string option = comments[i].Split(new string[] { "^" }, StringSplitOptions.None)[0];
                    hiddenOptions.Add(new SelectListItemInline()
                    {
                        Text = option.Trim(),
                        Value = option.Trim()
                    });
                }

                // distinct
                hiddenOptions = hiddenOptions.GroupBy(x => x.Text).Select(x => x.First()).ToList();
            }

            return hiddenOptions;
        }

        public void RemoveEmptyBarcode()
        {
            string[] barcodeElements = new string[] { "^B3" };
            foreach (var barcodeElement in barcodeElements)
            {
                RemoveEmptyBarcode(barcodeElement);
            }
        }

        private void RemoveEmptyBarcode(string barcodeElement)
        {
            string[] barcodes = Zpl.Split(new string[] { barcodeElement }, StringSplitOptions.None);
            for (int i = 1; i < barcodes.Count(); i++)
            {
                int FDPostion = barcodes[i].IndexOf("^FD");
                int FSPostion = barcodes[i].IndexOf("^FS");
                if (FDPostion < 0)
                {
                }
                if (FSPostion < 0)
                {
                }

                int barcodeStart = FDPostion + 3;
                int barcodeEnd = FSPostion;

                string barcodeValue = barcodes[i].Substring(barcodeStart, barcodeEnd - barcodeStart);

                if (string.IsNullOrWhiteSpace(barcodeValue))
                {
                    // remove the barcode element
                    barcodes[i] = barcodes[i].Substring(FDPostion);
                }
                else
                {
                    // restore the barcode data.
                    barcodes[i] = barcodeElement + barcodes[i];
                }
            }

            this.Zpl = string.Join("", barcodes);
        }
    }
}