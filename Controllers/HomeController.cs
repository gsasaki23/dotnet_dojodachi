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
            
            // "Mode" for the bottom status bit
            if (HttpContext.Session.GetString("Status") == null)
            {
                HttpContext.Session.SetString("Status","Regular");
            } 
            ViewBag.Status = HttpContext.Session.GetString("Status");

            return View();
        }

        [HttpGet("/reset")]
        public IActionResult Reset()
        {
            DojodachiModel dojodachi = new DojodachiModel();
            HttpContext.Session.SetObjectAsJson("DD",dojodachi);
            HttpContext.Session.SetString("Result","Welcome! Let's get started.");
            HttpContext.Session.SetString("Status","Regular");
            return RedirectToAction("Index");
        }
        
        [HttpGet("/feed")]
        public IActionResult Feed()
        {
            // Status logic
            DojodachiModel dojodachi = HttpContext.Session.GetObjectFromJson<DojodachiModel>("DD");
            string feedResult = dojodachi.feed();
            HttpContext.Session.SetObjectAsJson("DD",dojodachi);
            
            // Result
            if(feedResult == "noMeals"){
                HttpContext.Session.SetString("Result","Michael has no Meals left to be fed...");}
            else if(feedResult == "fail"){
                HttpContext.Session.SetString("Result","Michael didn't like the meal...");}
            else{
                HttpContext.Session.SetString("Result",$"Michael had a good meal! +{feedResult} Fullness, -1 Meal.");}
            
            // win condition
            if (dojodachi.gameWon())
            {
                HttpContext.Session.SetString("Result","Congratulations! You won and Michael is proud of you.");
                HttpContext.Session.SetString("Status","Over");
            }
            
            return RedirectToAction("Index");
        }
        
        [HttpGet("/play")]
        public IActionResult Play()
        {
            // Status logic
            DojodachiModel dojodachi = HttpContext.Session.GetObjectFromJson<DojodachiModel>("DD");
            string playResult = dojodachi.play();
            HttpContext.Session.SetObjectAsJson("DD",dojodachi);
            
            // Result
            if(playResult == "noEnergy"){
                HttpContext.Session.SetString("Result","Michael has no Energy left to play or code...");}
            else if(playResult == "fail"){
                HttpContext.Session.SetString("Result","Michael didn't like your code...");}
            else{
                HttpContext.Session.SetString("Result",$"You coded with Michael! +{playResult} Happiness, -5 Energy.");}
            
            // win/loss condition
            if (dojodachi.dead())
            {
                HttpContext.Session.SetString("Result","Michael has moved along...");
                HttpContext.Session.SetString("Status","Over");
            }
            if (dojodachi.gameWon())
            {
                HttpContext.Session.SetString("Result","Congratulations! You won and Michael is proud of you.");
                HttpContext.Session.SetString("Status","Over");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/work")]
        public IActionResult Work()
        {
            // Status logic
            DojodachiModel dojodachi = HttpContext.Session.GetObjectFromJson<DojodachiModel>("DD");
            string workResult = dojodachi.work();
            HttpContext.Session.SetObjectAsJson("DD",dojodachi);
            
            // Result
            if(workResult == "noEnergy"){
                HttpContext.Session.SetString("Result","Michael has no Energy left to make more videos...");}
            else{
                HttpContext.Session.SetString("Result",$"Michael made another video! +{workResult} Meals, -5 Energy.");}
            
            // loss condition
            if (dojodachi.dead())
            {
                HttpContext.Session.SetString("Result","Michael has moved along...");
                HttpContext.Session.SetString("Status","Over");
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet("/sleep")]
        public IActionResult Sleep()
        {
            // Status logic
            DojodachiModel dojodachi = HttpContext.Session.GetObjectFromJson<DojodachiModel>("DD");
            string sleepResult = dojodachi.sleep();
            HttpContext.Session.SetObjectAsJson("DD",dojodachi);
            
            // Result
            HttpContext.Session.SetString("Result",$"Michael had a good nap! +15 Energy, -5 Fullness, -5 Happiness.");
            
            // loss condition
            if (dojodachi.dead())
            {
                HttpContext.Session.SetString("Result","Michael has moved along...");
                HttpContext.Session.SetString("Status","Over");
            }
            
            return RedirectToAction("Index");
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
