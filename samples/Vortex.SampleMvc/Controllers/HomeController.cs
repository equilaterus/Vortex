using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Equilaterus.Vortex.Engine;
using Equilaterus.Vortex.Engine.Queries;
using Equilaterus.Vortex.Managers;
using Equilaterus.Vortex.Services;
using Microsoft.AspNetCore.Mvc;
using Vortex.SampleMvc.Models;

namespace Vortex.SampleMvc.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IPersistanceManager<ExampleModel> _persistanceManager;

        public HomeController(IPersistanceManager<ExampleModel> persistanceManager)
        {
            _persistanceManager = persistanceManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(
                await _persistanceManager.ExecuteQueryForEntities(
                    VortexEvents.RelationalQueryForEntities, 
                    new VortexData(
                        new RelationalQueryParams<ExampleModel>())));
        }

        public async Task<IActionResult> Add()
        {
            await _persistanceManager.ExecuteCommand(
                VortexEvents.InsertEntity,
                new VortexData(
                new ExampleModel
                {
                    Description = "Some random text... but always the same text."
                })
            );

            return RedirectToAction("Index");
        }
    }
}
