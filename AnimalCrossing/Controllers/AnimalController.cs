using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimalCrossing.Data;
using AnimalCrossing.Models;
using AnimalCrossing.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AnimalCrossing.Controllers
{
   // [Authorize]
    public class AnimalController : Controller
    {
        private IAnimalRepository animalRepository;
        private ISpeciesRepository speciesRepository;

        public AnimalController(IAnimalRepository animalRepo, ISpeciesRepository s)
        {
            this.speciesRepository = s;
            this.animalRepository = animalRepo;
        }


        // GET: /<controller>/
        [AllowAnonymous]
        public IActionResult Index(string searchString)
        {
            ViewBag.something = searchString;
            List<Cat> cats = this.animalRepository.Find(searchString);
            
            return View("ShowCats", cats.ToList());
        }

        public IActionResult Mates(Cat c)
        {
            List<Cat> cats = this.animalRepository.Mates(c);

            return View("ShowCats", cats.ToList());
        }


        public string Hello()
        {
            return "Well, hello there! We are learning .NET Core now...";
        }

        public IActionResult MyFirstView()
        {
            ViewBag.MyWifeSays = "Go buy groceries, clean up, make dinner";
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(ViewModelCreator.CreateAnimalCatVm(speciesRepository));
        }

        [HttpPost]
        public IActionResult Create(AnimalCatVM vm)
        {
            if (ModelState.IsValid) {
                ViewBag.Thanks = vm.Cat.Name;
                ViewBag.Cat = vm.Cat;

                animalRepository.Save(vm.Cat);

                return RedirectToAction("Mates", vm.Cat);
                
                //return View("Thanks", vm.Cat);
            }

            return View(ViewModelCreator.CreateAnimalCatVm(speciesRepository));

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Create an edit view
            // Look up cat object from catId in the database
            // Show an edit view to the user, displaying the cat object
            // Cat cat = animalRepository.Get(id);


            // return View(cat);

            if (id == null)
            {
                return NotFound();
            }
            var cat = animalRepository.Get(id);
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }

        [HttpPost]
        public IActionResult Edit(Cat c)
        {
            if (ModelState.IsValid)
            {
                animalRepository.Save(c);
                // Save it to the database
                return RedirectToAction("Index");
            }
            return View();
        }


    }
}
