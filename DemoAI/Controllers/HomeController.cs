using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoAI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;

namespace DemoAI.Controllers
{
    public class HomeController : Controller
    {
        private TelemetryClient _telemetry;

        public HomeController(TelemetryClient telemetry)
        {
            _telemetry = telemetry;
        }
        public IActionResult Index()
        {
            _telemetry.TrackTrace("HOME PAGE LOADED");
            _telemetry.TrackException(new Exception("CUSTOM EXCEPTION"));
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            bool success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                success = await DoSomeWorkAsync();
            }
            finally
            {
                timer.Stop();
                _telemetry.TrackDependency("myCustomDependency", "doSomeWork", null, startTime, timer.Elapsed, success);
            }
            _telemetry.TrackTrace("PRIVACY PAGE LOADED");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<bool> DoSomeWorkAsync() {
            await Task.Delay(1000);
            return true;
        }
    }
}
