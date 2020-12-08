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
    public class CartController : Controller
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(MenuItemController));


        [HttpGet("{id}")]

        public async Task<ActionResult> AddToCart(int id)
        {
            if (HttpContext.Session.GetString("token") == null)
            {
                _log4net.Error("token not found");

                return RedirectToAction("Login", "Login");

            }
            var userid = HttpContext.Session.GetInt32("Userid");
            _log4net.Info("Http get request initiated for add to cart");
            MenuItem menuItem = new MenuItem();
            using (var client = new HttpClient())
            {
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                //3localhost:44356/api/organization,353"https ://localhost:44353/api/organization"
                using (var response = await client.GetAsync("https://localhost:44382/api/MenuItem" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    menuItem = JsonConvert.DeserializeObject<MenuItem>(apiResponse);
                }
                StringContent content = new StringContent(JsonConvert.SerializeObject(menuItem), Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync("https://localhost:44372/api/Cart/" + userid, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    menuItem = JsonConvert.DeserializeObject<MenuItem>(apiResponse);
                }


            }

            return RedirectToAction("Index", "MenuItem");
        }
        // GET: CartController

        [HttpGet]
        public async Task<ActionResult> GetCartItems()
        {
            if (HttpContext.Session.GetString("token") == null)
            {
                _log4net.Error("token not found");

                return RedirectToAction("Login", "Login");

            }
            int? id = HttpContext.Session.GetInt32("Userid");


            _log4net.Info("Http get request initiated for Getting cart items");
            List<MenuItem> menuItems = new List<MenuItem>();
            using (var client = new HttpClient())
            {
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                using (var response = await client.GetAsync("https://localhost:44372/api/Cart/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    menuItems = JsonConvert.DeserializeObject<List<MenuItem>>(apiResponse);
                }


            }
            var totalPrice = 0.0;
            foreach (var item in menuItems)
            {
                totalPrice += item.Price;
            }
            ViewBag.Price = totalPrice;
            return View(menuItems);

        }
        [HttpGet("byUser")]
        public async Task<ActionResult> PlaceOrderAsync()
        {
            if (HttpContext.Session.GetString("token") == null)
            {
                _log4net.Error("token not found");

                return RedirectToAction("Login", "Login");

            }
            using (var client = new HttpClient())
            {
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                var userid = HttpContext.Session.GetInt32("Userid");
                using (var response = await client.GetAsync("https://localhost:44372/api/Cart/user?userid=" + userid))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                  
                }
              


            }
            return View();
        }
     
        [HttpGet]
       
        public async Task<ActionResult> DeleteItem(int id,int user)
        {
            if (HttpContext.Session.GetString("token") == null)
            {
                _log4net.Error("token not found");

                return RedirectToAction("Login", "Login");

            }
            int? userid = HttpContext.Session.GetInt32("Userid");
            _log4net.Info("Http get request initiated to delete item from the cart");

            using (var client = new HttpClient())
            {
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");

                using (var response = await client.DeleteAsync("https://localhost:44372/api/Cart/" + userid+ "?menuitemid=" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                   
                }


                return RedirectToAction("GetCartItems");
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: CartController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CartController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
