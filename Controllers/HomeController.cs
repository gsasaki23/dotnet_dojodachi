using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dojodachi.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace dojodachi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Very first render: make Dojodachi obj
            if (HttpContext.Session.GetObjectFromJson<DojodachiModel>("DD") == null)
            {
                DojodachiModel dojodachi = new DojodachiModel();
                HttpContext.Session.SetObjectAsJson("DD",dojodachi);
            }
            ViewBag.DD = HttpContext.Session.GetObjectFromJson<DojodachiModel>("DD");

            // Very first render: start with a different line
            if (HttpContext.Session.GetString("Result") == null)
            {
                HttpContext.Session.SetString("Result","Welcome! Let's get started.");
            } 
            ViewBag.Result = HttpContext.Session.GetString("Result");
            
            // "Mode" for the bottom partials
            if (HttpContext.Session.GetString("Partial") == null)
            {
                HttpContext.Session.SetString("Partial","Regular");
            } 
            ViewBag.Partial = HttpContext.Session.GetString("Partial");

            return View();
        }

        [HttpGet("/feed")]
        public IActionResult Feed()
        {
            // run feed logic on Dojodachi obj
            DojodachiModel dojodachi = new DojodachiModel();
            return RedirectToAction("Index", dojodachi);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
