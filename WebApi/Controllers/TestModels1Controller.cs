using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class TestModels1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TestModels1
        public ActionResult Index()
        {
            if (Session["UserToken"] != null)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (Session["UserToken"] as UserToken).access_token);
                HttpResponseMessage response = client.GetAsync("http://localhost:56491/api/TestModels").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                if (response.ReasonPhrase == "Internal Server Error")
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
                    return View(JsonConvert.DeserializeObject<List<TestModel>>(result));
            }
           else
            {
                return RedirectToAction("Login", "AccountM", new { returnUrl = Request.Url.AbsolutePath });
            }
        }

        // GET: TestModels1/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (Session["UserToken"] as UserToken).access_token);
            HttpResponseMessage response = client.GetAsync($"http://localhost:56491/api/TestModels?id={id}").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            if (response.ReasonPhrase == "Internal Server Error")
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            if(response.IsSuccessStatusCode)
            {
                return View(JsonConvert.DeserializeObject<TestModel>(result));
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        // GET: TestModels1/Create
        public ActionResult Create()
        {
            if (Session["UserToken"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "AccountM", new { returnUrl = Request.Url.AbsolutePath });
            }
        }

        // POST: TestModels1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Age")] TestModel testModel)
        {
            
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (Session["UserToken"] as UserToken).access_token);
                HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://localhost:56491/api/TestModels/Create") { Content = new StringContent(JsonConvert.SerializeObject(testModel), System.Text.Encoding.UTF8, "application/json") }).Result;
                string result = response.Content.ReadAsStringAsync().Result;
            if (response.ReasonPhrase == "Internal Server Error")
            {
                ModelState.AddModelError("", "Internal Server Error");
                return View(testModel);
            }
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id=result.Replace("\"","")});
            }
            else
            {
                return View(testModel);

            }
        }

        // GET: TestModels1/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (Session["UserToken"] as UserToken).access_token);
            HttpResponseMessage response = client.GetAsync($"http://localhost:56491/api/TestModels?id={id}").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            if (response.ReasonPhrase == "Internal Server Error")
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            if (response.IsSuccessStatusCode)
            {
                return View(JsonConvert.DeserializeObject<TestModel>(result));
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        // POST: TestModels1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Age")] TestModel testModel)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (Session["UserToken"] as UserToken).access_token);
            HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Put, $"http://localhost:56491/api/TestModels?id={testModel.ID}") { Content = new StringContent(JsonConvert.SerializeObject(testModel), System.Text.Encoding.UTF8, "application/json") }).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            if (response.ReasonPhrase == "Internal Server Error")
            {
                ModelState.AddModelError("", "Internal Server Error");
                return View(testModel);
            }
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = result.Replace("\"", "") });
            }
            else
            {
                return View(testModel);

            }
        }

        // GET: TestModels1/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (Session["UserToken"] as UserToken).access_token);
            HttpResponseMessage response = client.GetAsync($"http://localhost:56491/api/TestModels?id={id}").Result;
            string result = response.Content.ReadAsStringAsync().Result;
            if (response.ReasonPhrase == "Internal Server Error")
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            if (response.IsSuccessStatusCode)
            {
                return View(JsonConvert.DeserializeObject<TestModel>(result));
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        // POST: TestModels1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (Session["UserToken"] as UserToken).access_token);
            HttpResponseMessage response = client.DeleteAsync($"http://localhost:56491/api/TestModels?id={id}").Result;
            if (response.ReasonPhrase == "Internal Server Error")
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            if (response.IsSuccessStatusCode)
            {
               return RedirectToAction("index");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
