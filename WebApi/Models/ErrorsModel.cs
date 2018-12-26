using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ModelStateDictionary
    {
        public string Message { get; set; }
        public Modelstate ModelState { get; set; }
    }

    public class Modelstate
    {
        public string[] modelEmail { get; set; }
        public string[] modelPassword { get; set; }
        public string[] modelConfirmPassword { get; set; }
        public string[] modelName { get; set; }
        public string[] modelPhone { get; set; }
        public string[]  error { get; set; }
    }
    public class LoginErrorModel
    {
        public string error { get; set; }
        public string error_description { get; set; }
    }
}