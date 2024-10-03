using MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private string RenderPartialViewToString(string viewName, object model)
        {
            // Check if the viewName is provided; if not, use the action name
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            // Assign the model to ViewData
            ViewData.Model = model;

            // Use StringWriter to capture the rendered view
            using (var sw = new StringWriter())
            {
                // Find the partial view using the controller's ViewEngine
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

                // Create a ViewContext to render the view
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                // Return the rendered view as a string
                return sw.GetStringBuilder().ToString();
            }
        }
        // GET: User
        public ActionResult Index()
        {
            List<MVCUserModel> userlist = new List<MVCUserModel>();
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Users").Result;
            //userlist = response.Content.ReadAsAsync<IEnumerable<MVCUserModel>>().Result;
            //return View(userlist);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                userlist = JsonConvert.DeserializeObject<List<MVCUserModel>>(data);
                return View(userlist);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(MVCUserModel User)
        {

            if (User.UserID == 0)
            {
                List<MVCUserModel> userlist = new List<MVCUserModel>();
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Users", User).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                userlist = JsonConvert.DeserializeObject<List<MVCUserModel>>(data);
                var html = RenderPartialViewToString("_UserAddPartial", userlist);
                return Json(new { success = true, html = html });
            }
            else {
                List<MVCUserModel> userlist = new List<MVCUserModel>();
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Users/"+User.UserID, User).Result;
                string data = response.Content.ReadAsStringAsync().Result;
                userlist = JsonConvert.DeserializeObject<List<MVCUserModel>>(data);
                var html = RenderPartialViewToString("_UserAddPartial", userlist);
                return Json(new { success = true, html = html });
            }
          


        }

        [HttpPost]
        public ActionResult Delete(int id)
        {

            List<MVCUserModel> userlist = new List<MVCUserModel>();
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Users/"+id).Result;
            string data = response.Content.ReadAsStringAsync().Result;
            userlist = JsonConvert.DeserializeObject<List<MVCUserModel>>(data);
            var html = RenderPartialViewToString("_UserAddPartial", userlist);
            return Json(new { success = true, html = html });

            //return RedirectToAction("Index");
        }
    }
}