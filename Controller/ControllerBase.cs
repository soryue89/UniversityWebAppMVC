using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Park_University_MVC.Controllers
{
    public class ControllerBase : Controller
    {
        public string SerializeControl(string controlPath, object model)
        {
            ViewResult v = View();
            if (String.IsNullOrEmpty(v.ViewName))
                v.ViewName = RouteData.GetRequiredString("action");
            ViewEngineResult result = null;
            StringBuilder sb = new StringBuilder();
            StringWriter textWriter = new StringWriter(sb);
            HtmlTextWriter htmlWriter = new HtmlTextWriter(textWriter);
            if (v.View == null)
            {
                result = new ViewEngineResult(new RazorView(this.ControllerContext, controlPath, "", false, null), new RazorViewEngine());
                v.View = result.View;
            }
            ViewContext viewContext = new ViewContext(ControllerContext, v.View, ViewData, TempData, htmlWriter);
            viewContext.ViewData.Model = model;
            v.View.Render(viewContext, htmlWriter);
            string rval = sb.ToString();
            htmlWriter.Close();
            textWriter.Close();
            return rval;
        }

        public JsonResult ReturnJsonGet(string code, string result, string errors)
        {
            return Json(new { code = code, data = result, errors = errors }, JsonRequestBehavior.AllowGet);
        }
    } 
}
