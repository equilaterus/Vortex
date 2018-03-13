using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Equilaterus.Vortex;
using Microsoft.AspNetCore.Mvc;
using Vortex.SampleMvc.Models;

namespace Vortex.SampleMvc.Controllers
{
    public class HomeController : Controller
    {
        protected readonly ICrudBehavior<ExampleModel> _behavior;

        public HomeController(ICrudBehavior<ExampleModel> behavior)
        {
            _behavior = behavior;
        }

        public async Task<IActionResult> Index()
        {
            return View(
                await _behavior.FindAllAsync());
        }

        public async Task<IActionResult> Add()
        {
            await _behavior.InsertAsync(
                new ExampleModel
                {
                    Description = "Some random text... but always the same text."
                }
            );

            return RedirectToAction("Index");
        }
    }
}
