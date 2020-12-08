using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TruYumClient.Models;

namespace TruYumClient.Controllers
{
    public class LoginController : Controller
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(LoginController));
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //https://localhost:44304/api/Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {

            _log4net.Info("User Login");
            User Item = new User();
            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");






                StringContent content1 = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var response1 = await httpClient.PostAsync("https://localhost:44304/api/Auth/Login", content1))
                {
                    if (!response1.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login");
                    }

                    string apiResponse1 = await response1.Content.ReadAsStringAsync();

                    Jwt jwt = JsonConvert.DeserializeObject<Jwt>(apiResponse1);

                    HttpContext.Session.SetString("token", jwt.Token);
                    HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                    HttpContext.Session.SetInt32("Userid", user.Userid);
                    HttpContext.Session.SetString("Username", user.Username);
                    ViewBag.Message = "User logged in successfully!";

                    return RedirectToAction("Index", "MenuItem");


                }
            }
        }
        public ActionResult Logout()
        {
            _log4net.Info("User Log Out");
            HttpContext.Session.Remove("token");
            

            return View("Login");
        }


       
    }
}
