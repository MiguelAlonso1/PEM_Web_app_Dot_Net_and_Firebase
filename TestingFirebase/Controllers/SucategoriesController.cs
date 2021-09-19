using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFirebase.Models;

namespace TestingFirebase.Controllers
{
    public class SubcategoriesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var firebaseClient = new FirebaseClient("https://med-app-519aa.firebaseio.com/");

            //var result = await firebaseClient
            //.Child("Users/" + userId + "/Logins")
            //.PostAsync(currentUserLogin);

            //Retrieve data from Firebase
            var subCatList = await firebaseClient
            .Child("categories")
            .OrderByKey()
            .OnceAsync<Subcategory>();

            //var subCategoryObj = subCatList.Where(x => x.Object.SubId.Equals( "c1") );
            var subCategoryObj = subCatList;
            var subCategoryList = new List<Subcategory>();

            /// Convert JSON data to original datatype
            foreach (var subCategory in subCategoryObj)
            {
                    subCategoryList.Add(new Subcategory
                    {
                        Key = Convert.ToString(subCategory.Key),
                        Title = Convert.ToString(subCategory.Object.Title),
                        SubId = Convert.ToString(subCategory.Object.SubId),
                       // ImageUrl = @"\images\img.png"//Verbatim string for pic
                    });

                Console.WriteLine($"{subCategory.Key} is");
                Console.WriteLine($"Evaluation is {subCategory.Object.Evaluation}");
                Console.WriteLine();
            }
            @ViewBag.subCatList = 0;
            return View(subCategoryList);
        }

        public async Task<IActionResult> Details(string id)
        {
            var firebaseClient = new FirebaseClient("https://med-app-519aa.firebaseio.com/");

            //var result = await firebaseClient
            //.Child("Users/" + userId + "/Logins")
            //.PostAsync(currentUserLogin);

            //Retrieve ALL Kategories from Firebase
            var subCatList = await firebaseClient
            .Child("categories")
            .OrderByKey()
            .OnceAsync<Subcategory>();

            var firebaseObj = subCatList.FirstOrDefault(x => x.Key.Equals( id) );

            /// Convert JSON data to original datatype
          
             Subcategory obj = new Subcategory
                {
                    Key = Convert.ToString(firebaseObj.Key),
                    SubId = Convert.ToString(firebaseObj.Object.SubId),
                    Title = Convert.ToString(firebaseObj.Object.Title),
                    Color = Convert.ToString(firebaseObj.Object.Color),
                    Evaluation = Convert.ToString(firebaseObj.Object.Evaluation),
                    Management = Convert.ToString(firebaseObj.Object.Management),
                    Medications = Convert.ToString(firebaseObj.Object.Medications),
                    Signs = Convert.ToString(firebaseObj.Object.Management),
                    References = Convert.ToString(firebaseObj.Object.Medications),
                 // ImageUrl = @"\images\img.png"//Verbatim string for pic
             };
          
            @ViewBag.subCatList = 0;
            return View(obj);
        }

        //GET-Create
        public IActionResult Create()
        {
            var mainKatArray = new string[] { "c1", "c2", "c3", "c4", "c5" };

            IEnumerable<SelectListItem> mainKategoryNameValues =
                mainKatArray.Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                }); ;

            //ViewBag is automatically passed to the view
            ViewBag.MainKategoryListOfNames = mainKategoryNameValues;

            return View();//returns the HTML form
        }

        #region ::Http is Web access. Web access benefits from asynchronous programming::
        // need to research this
        #endregion
        ////POST-Create-always use POST for delete update and authentication for security reasons!
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subcategory obj)//obj comes form the HTML form
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            
            var firebaseClient = new FirebaseClient("https://med-app-519aa.firebaseio.com/");
            var result = await firebaseClient
            .Child("categories")
            .PostAsync(obj);
            return RedirectToAction("Index", new { Id = obj.SubId });//to the Index in Categories since this controller is in Categories
        }

        //GET-Update
        public IActionResult Update(string id)//obj comes form the HTML form
        {
            if (id == null)//in Sql table, all IDs start from 1
            {
                return NotFound();
            }
            var obj = _db.SubCategories.Find(id);//Find matches on Primary Key only

            if (obj == null)
            {
                return NotFound();
            }

            MainCategory mainKategoryList = _db.MainCategories.FirstOrDefault(x => x.Id == obj.MainCategoryId);
            if (mainKategoryList == null)
            {
                return NotFound();
            }

            SelectListItem mainKategorySelectList =
                 new SelectListItem
                 {
                     Text = mainKategoryList.CategoryName,
                     Value = mainKategoryList.Id.ToString()
                 };

            #region ::The below list is needed since...::

            /*The below list is needed since the HTML select list asp-items control
             wants an object that resolves as an IEnumerable so it can iterate throgh it
            to render the option HMTL tags for some reason creating a IEnumerable variable
            to assign it the list item didn't work. Maybe cuz it's an abstract object
            The list below resolves as an IEnumerable so this worked
             */
            #endregion
            List<SelectListItem> sl = new List<SelectListItem>();
            sl.Add(mainKategorySelectList);

            //ViewBag is automatically passed to the view
            ViewBag.MainKategorySelectList = sl;
            return View(obj);
        }
       
        #region ::Http is Web access. Web access benefits from asynchronous programming::
        // need to research this
        #endregion
        //POST-Update-always use POST for delete update and authentication for security reasons!
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Subcategory obj)//obj comes form the HTML form
        {
            //if (obj == null)
            //{
            //    return NotFound();
            //}
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            _db.SubCategories.Update(obj);
            _db.SaveChanges();

            #region ::The second parameter is just part...::
            /*
            the second parameter is just part of an object called route values. Id is a part of it
            would look something like this
            return RedirectToAction( "Main", new RouteValueDictionary( 
            new { controller = controllerName, action = "Main", Id = Id } ) );
            */
            #endregion

            return RedirectToAction("Index", new { id = obj.MainCategoryId });//to the Index in Subcategories since this controller is in Categories
        }
    }
}