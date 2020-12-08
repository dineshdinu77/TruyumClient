using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TruYumClient.Models;

namespace TruYumClient.Controllers
{
    public class MenuItemController : Controller
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(MenuItemController));
        // GET: MenuItemController
        public async Task<ActionResult> IndexAsync()
        {
            if (HttpContext.Session.GetString("token") == null)
            {
                _log4net.Error("token not found");

                return RedirectToAction("Login", "Login");

            }
            _log4net.Info("Http get request initiated for organization details");
            List<MenuItem> menuItems = new List<MenuItem>();
            using (var client = new HttpClient())
            {
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
               // http ://20.62.212.37/swagger/index.html
                using (var response = await client.GetAsync("https://localhost:44382/api/menuitem"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    menuItems = JsonConvert.DeserializeObject<List<MenuItem>>(apiResponse);
                }

            }


            return View(menuItems);
        }

      

        // GET: MenuItemController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MenuItemController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(MenuItem menuItem)
        {
            try
            {
                if (HttpContext.Session.GetString("token") == null)
                {
                    _log4net.Info("token not found");

                    return RedirectToAction("Login", "Login");

                }
                _log4net.Info("http post request initiaited for posting organization");
                using (var httpclinet = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(menuItem), Encoding.UTF8, "application/json");
                    using (var response = await httpclinet.PostAsync("https://localhost:44382/api/MenuItem", content))
                    {

                        string apiResponse = await response.Content.ReadAsStringAsync();
                        menuItem = JsonConvert.DeserializeObject<MenuItem>(apiResponse);
                    }
                }
                return RedirectToAction("Index");
               
            }
            catch
            {
                return View();
            }
        }
      

      



       
    }
}
