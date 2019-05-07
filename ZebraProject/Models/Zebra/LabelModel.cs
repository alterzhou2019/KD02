using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ZebraProject.Models.Zebra
{
    public abstract class LabelModel
    {
        /// <summary>
        /// 传入的值跟zebra里面的值相替换
        /// </summary>
        /// <param name="zpl"></param>
        /// <returns></returns>
        virtual public string FillTemplateValue(string zpl)
        {
            Type type = this.GetType();
            IList<PropertyInfo> propertyList = new List<PropertyInfo>(type.GetProperties());

            foreach (PropertyInfo property in propertyList)
            {
                object value = property.GetValue(this, null);
                if (value == null)
                {
                    value = "";
                }
                zpl = zpl.Replace("[" + property.Name + "]", value.ToString());
            }
            return zpl;
        }

        virtual public List<string> GetZPLList()
        {
            List<string> zpls = new List<string>();
            return zpls;
        }

    }
}