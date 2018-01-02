using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Equilaterus.Vortex.Services;
using Microsoft.AspNetCore.Mvc;
using Vortex.SampleMvc.Models;

namespace Vortex.SampleMvc.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IDataStorage<ExampleModel> _dataStorage;

        public HomeController(IDataStorage<ExampleModel> dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dataStorage.FindAllAsync());
        }

        public async Task<IActionResult> Add()
        {
            await _dataStorage.InsertAsync(
                new ExampleModel
                {
                    Description = "Some random text... but always the same text."
                }
            );

            return RedirectToAction("Index");
        }
    }
}
