using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Data;

namespace FileUploadExample.Controllers
{
    public class SingleFileController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _dbContext;


        public SingleFileController(IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            _env = env;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            string[] filespaths = Directory.GetFiles(Path.Combine(this._env.WebRootPath, "Uploads/"));
            List<Dokument> list = new List<Dokument>();
            foreach (string filepath in filespaths)
            {
                list.Add(new Dokument { Namn = Path.GetFileName(filepath) });


            }
            return View(list);
        }
        public FileResult DownloadFile(string filename)
        {
            string path = Path.Combine(this._env.WebRootPath,"Uploads/")+filename;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", filename);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Dokument item)
        {
            //Either use a viewmodel or remove photoURL. URL will
            //be generated programmatically
            ModelState.Remove(nameof(Dokument.Path));
            item.Id = 0; //SQL Server will generate a new ID

            if (ModelState.IsValid)
            {
                IFormFile file = item.File;
                //Check file extension is a photo or text
                string extension =
                       Path.GetExtension(file.FileName);
                if (extension == ".png" || extension == ".jpg"||extension==".txt")
                {

                    //generate unique name to retrieve later
                    //string newFileName = Guid.NewGuid().ToString();
                    string newFileName = item.Namn;



                    //store photo on file system and reference in DB
                    if (file.Length > 0) //ensure the file is not empty
                    {
                        string filePath = Path.Combine(_env.WebRootPath, "Uploads"
                                                    , newFileName + extension);
                        //save location to database (in URL format)
                        item.Path = "Uploads/" + newFileName + extension;
                       
                      
                        //write file to file system
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                        TempData["msg"] = "File Uploaded successfully";
                        }
                       // item.AnvandareId
                        _dbContext.Dokument.Add(item);
                        _dbContext.SaveChanges();
                        // return RedirectToAction("Index", "SingleFile");
                        return View();

                    }

                    return View();
                }

            }

            return View();
        }
    }
}