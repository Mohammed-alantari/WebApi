using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;
namespace WebApi.Controllers
{
    public class AccountMController : Controller
    {
        // GET: AccountM
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterBindingModel RegisterBindingModel)
        {

            var Keys = new Dictionary<string, string>
            {
                { "Name",RegisterBindingModel.Name},
                { "Email",RegisterBindingModel.Email},
                { "Phone",RegisterBindingModel.Phone},
                { "Password",RegisterBindingModel.Password},
                { "ConfirmPassword",RegisterBindingModel.ConfirmPassword}

            };
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://localhost:56491/api/Account/Register") { Content = new FormUrlEncodedContent(Keys) }).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            if(response.ReasonPhrase== "Internal Server Error")
            {
                ModelState.AddModelError("", "Internal Server Error");
                return View();

            }
            if (response.IsSuccessStatusCode)
            {

                var keyValues = new Dictionary<string, string>
               {
                { "username",RegisterBindingModel.Email },
                { "password",RegisterBindingModel.Password },
                { "grant_type", "password" }
               };
                var client1 = new HttpClient();
                var response1 = client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://localhost:56491/Token")
                {
                    Content = new FormUrlEncodedContent(keyValues)
                }).Result;
                var result1 = response1.Content.ReadAsStringAsync().Result;
                if (response1.ReasonPhrase == "Internal Server Error")
                {
                    ModelState.AddModelError("", "Internal Server Error");
                    return View(RegisterBindingModel);
                }
                if (response1.IsSuccessStatusCode)
                {

                    Session["UserToken"] = JsonConvert.DeserializeObject<UserToken>(result1);
                    return RedirectToAction("index","Home");
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<Models.LoginErrorModel>(result1);
                    ModelState.AddModelError("", error.error_description);
                }

                return View(RegisterBindingModel);
            }
            else
            {
                
                    string editresult = result.Replace("model.Email", "modelEmail").Replace("model.Password", "modelPassword").Replace("model.Name", "modelName").Replace("model.Phone", "modelPhone").Replace("model.ConfirmPassword", "modelConfirmPassword");
                    var error = JsonConvert.DeserializeObject<WebApi.Models.ModelStateDictionary>(editresult).ModelState;

                    foreach (var err in error.GetType().GetProperties())
                    {
                        var value = err.GetValue(error);
                        
                        if (value != null)
                        {
                        foreach (var innererr in value as string[])
                        {
                            ModelState.AddModelError("", innererr);

                        }
                        }
             
                   
                    }
                return View();
              
            }
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string returnUrl,LoginModel login)
        {
            var keyValues = new Dictionary<string, string>
               {
                { "username",login.Email },
                { "password",login.Password },
                { "grant_type", "password" }
               };
            var client = new HttpClient();
            var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://localhost:56491/Token")
            {
                Content = new FormUrlEncodedContent(keyValues)
            }).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            if (response.ReasonPhrase == "Internal Server Error")
            {
                ModelState.AddModelError("", "Internal Server Error");               
                return View(login);
            }
            if(response.IsSuccessStatusCode)
            {
               
                Session["UserToken"] = JsonConvert.DeserializeObject<UserToken>(result);
                return RedirectToLocal(returnUrl);
            }
            else
            {
            var  error= JsonConvert.DeserializeObject<Models.LoginErrorModel>(result);
                ModelState.AddModelError("",error.error_description);

            return View(login);
            }

        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}