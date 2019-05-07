using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZebraProject.Models.Zebra;

namespace ZebraProject.Controllers
{
    public class ZebraController : Controller
    {
        public ActionResult Index()
        {
            ZplModel model = new ZplModel();
            model.GetPreviewOption();
            return View();
        }


    }
}