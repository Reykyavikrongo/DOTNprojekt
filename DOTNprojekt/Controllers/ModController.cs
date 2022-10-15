using DOTNprojekt.Data;
using DOTNprojekt.Models;
using DOTNprojekt.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;


namespace DOTNprojekt.Controllers
{
    public class ModController : Controller
    {
        // Class that is tasked with managing all functions related to mods
        private readonly ApplicationDbContext _db;
        public ModController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            //Send all mods as a list to the view
            IEnumerable<Mod> objModList = _db.Mods.ToList();
            return View(objModList);
        }

        public IActionResult SearchIndex()
        {
            // Using ViewModel, send the data the user searched for to the view
            // Deserialize the data sent from Search() function
            List<Mod> mods = JsonConvert.DeserializeObject<List<Mod>>(TempData["list"].ToString());
            IEnumerable<Mod> enumMods = mods;
            List<PresentationModVM> PMVMlist = new List<PresentationModVM>();
            string TempUploaderName = "";
            foreach (Mod mod in mods)
            {
                // Eager loading for the database to fetch data for the foreign keys as well as regular data
                var modsIterator = _db.Mods.Include(mod => mod.Uploader).ToList();
                foreach (Mod modUploaderCheck in modsIterator)
                {
                    if (modUploaderCheck.Mod_Id == mod.Mod_Id)
                    {
                        TempUploaderName = modUploaderCheck.Uploader.User_Name;
                    }

                }
                // Searching through uploaded files to display the mod
                string path = "wwwroot/UploadedFiles/" + TempUploaderName + "/" + mod.Mod_Name + "/";
                PresentationModVM PMVM = new PresentationModVM();
                PMVM.ZipName = mod.Mod_Name;
                PMVM.UploaderName = TempUploaderName;
                PMVM.Downloads = mod.n_Downloads;
                PMVM.Name = "";
                string[] items = Directory.GetFiles(path);
                foreach (var item in items)
                {
                    if (PMVM.image || item.EndsWith(".png") || item.EndsWith(".jpg") || item.EndsWith(".jpeg"))
                    {
                        PMVM.Name = "~/" + item.Substring(8);
                        PMVM.image = true;
                    }
                    else 
                        PMVM.image = false;
                }
                PMVMlist.Add(PMVM);
            }
            //Send the ViewModel to the view
            IEnumerable<PresentationModVM> PMVMEnum = PMVMlist;
            return View(PMVMEnum);
        }

        public IActionResult UploadedSuccessfully()
        {
            return View();
        }

        // GET
        public IActionResult Search()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(SelectionVM svm)
        {
            //Once the user hits search the chosen options will be used to filter through the mods
            List<Mod> objModList = new List<Mod>();
            if (svm.selectedCategory == "Any" && svm.selectedCharacter == "Any")
            {
                objModList = _db.Mods.ToList();
            }
            else if (svm.selectedCharacter != "Any" && svm.selectedCategory == "Any")
            {
                foreach (Mod mod in _db.Mods)
                {
                    if (mod.Mod_Char == svm.selectedCharacter)
                        objModList.Add(mod);
                }
            }
            else if (svm.selectedCategory != "Any" && svm.selectedCharacter == "Any")
            {
                foreach (Mod mod in _db.Mods)
                {
                    if (mod.Mod_cat == svm.selectedCategory)
                        objModList.Add(mod);
                }
            }
            else
            {
                foreach (Mod mod in _db.Mods)
                {
                    if (mod.Mod_cat == svm.selectedCategory && mod.Mod_Char == svm.selectedCharacter)
                        objModList.Add(mod);
                }
            }
            // The filtered mods added to TempData to be sent to SearchIndex() function
            TempData["list"] = JsonConvert.SerializeObject(objModList);
            return RedirectToAction("SearchIndex");
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(ModVM obj)
        {
            // Check if the uploaded file meets the upload requirements
            if (ModelState.IsValid)
            {
                foreach (var mod in _db.Mods.ToList())
                {
                    if (mod.Mod_Name == obj.Mod_Name)
                    {
                        ModelState.AddModelError("Mod_Name", "This mod name is already in use");
                        return View(obj);
                    }
                }
                if (obj.Mod_File.FileName.EndsWith("zip"))
                {
                    // Define parameters for the mod that will be uploaded
                    Mod toAdd = new Mod();
                    toAdd.Mod_Name = obj.Mod_Name;
                    toAdd.Mod_Char = obj.Mod_Char;
                    toAdd.Mod_cat = obj.Mod_cat;
                    User temp = new User();
                    // Check the username of the currently logged in user
                    temp.User_Name = HttpContext.Session.GetString("login_name");

                    foreach (var user in _db.Users.ToList())
                    {
                        if (user.User_Name == temp.User_Name)
                        {
                            toAdd.Uploader = user;
                            toAdd.Uploader.n_Uploads = user.n_Uploads;
                        }
                    }
                    // Send parameters to the function that actually uploads the file
                    UploadFile(obj.Mod_File, obj.Mod_Name, toAdd.Uploader.User_Name);

                    toAdd.Uploader.n_Uploads++;
                    _db.Mods.Add(toAdd);
                    _db.SaveChanges();
                    return RedirectToAction("UploadedSuccessfully");
                }
                else
                {
                    ModelState.AddModelError("Mod_File", "Must be a .zip file");
                }
                
            }
            return View(obj);
        }


        public async Task<bool> UploadFile(IFormFile file, string ModName, string uploaderName)
        {
            // Async function that creates the location/directory where the mod files will be stored
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot//UploadedFiles//" + uploaderName + "//" + ModName));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    // Moving to the directory
                    using (var fileStream = new FileStream(Path.Combine(path, ModName + ".zip"), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    string ZipPath = path + "//" + ModName + ".zip";
                    // Extracting into said directory
                    ZipFile.ExtractToDirectory(ZipPath,path);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Mod_File", "File Upload Failed");
            }
            return false;
        }
    }
}
