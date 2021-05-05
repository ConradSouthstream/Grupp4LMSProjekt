//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.IO;
//using Microsoft.AspNetCore.Hosting;
//using LMS.Grupp4.Core.Entities;

//namespace FileUploadExample.Controllers
//{
//    public class SingleFileController : Controller
//    {
//        private readonly IWebHostEnvironment _env;

//        public SingleFileController(IWebHostEnvironment env)
//        {
//            _env = env;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Index(Dokument item)
//        {
//            //Either use a viewmodel or remove photoURL. URL will
//            //be generated programmatically
//            ModelState.Remove(nameof(Dokument.Path));
//            item.Id = 0; //SQL Server will generate a new ID

//            if (ModelState.IsValid)
//            {
//                IFormFile file = item.File;
//                //Check file extension is a photo
//                string extension =
//                       Path.GetExtension(file.FileName);
//                if (extension == ".png" || extension == ".jpg")
//                {
//                    //TODO: Use ImageSharp to resize uploaded photo
//                    //https://www.hanselman.com/blog/HowDoYouUseSystemDrawingInNETCore.aspx

//                    //generate unique name to retrieve later
//                    string newFileName = Guid.NewGuid().ToString();

//                    //store photo on file system and reference in DB
//                    if (file.Length > 0) //ensure the file is not empty
//                    {
//                        string filePath = Path.Combine(_env.WebRootPath, "images"
//                                                    , newFileName + extension);
//                        //save location to database (in URL format)
//                        item.Path = "images/" + newFileName + extension;
//                        //write file to file system
//                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
//                        {
//                            await file.CopyToAsync(fs);
//                        }
//                        return RedirectToAction("Index", "Home");
//                    }

//                    return View();
//                }

//            }

//            return View();
//        }
//    }
//}