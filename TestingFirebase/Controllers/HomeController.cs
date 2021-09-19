using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestingFirebase.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace TestingFirebase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
    
            //var result = await firebaseClient
            //.Child("Users/" + userId + "/Logins")
            //.PostAsync(currentUserLogin);
            var firebaseClient = new FirebaseClient("https://med-app-519aa.firebaseio.com/");

            //Retrieve data from Firebase
            var dbLogins = await firebaseClient
            .Child("categories")
            .OrderByKey()
            //.LimitToFirst(2)
            .OnceAsync<MainCategory>();

            var mainCategoriesList = new List<MainCategory>();

            var mainKatIds  = new string []{"c1", "c2","c3","c4","c5"};

            /// Convert JSON da ta to original datatype, then make a MainCategory and add it to list

            //foreach (var mainCategory in dbLogins)
            //{
            //    mainCategoriesList.Add(new MainCategory
            //    {
            //      Key = Convert.ToString(mainCategory.Key),
            //      Title = Convert.ToString(mainCategory.Object.Title),
            //        CategoryDescription = Convert.ToString(mainCategory.Object.CategoryDescription),
            //      ImageUrl = @"\images\img.png"//Verbatim string for pic
            //    });

            //    Console.WriteLine($"{mainCategory.Key} is");
            //    Console.WriteLine($"Evaluation is {mainCategory.Object.CategoryDescription}");
            //    Console.WriteLine();
            //}
            foreach (var val in mainKatIds)
            {
                mainCategoriesList.Add(new MainCategory
                {
                    Key = val,//use to make asp-route-id for subcategories
                    Title = val,
                    CategoryDescription = "Insert description",
                    ImageUrl = @"\images\img.png"//Verbatim string for pic
                });

                //Console.WriteLine($"{mainCategory.Key} is");
                //Console.WriteLine($"Evaluation is {mainCategory.Object.CategoryDescription}");
                //Console.WriteLine();
            }

            @ViewBag.subCatList = 0;
            return View(mainCategoriesList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
