using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LMS.Grupp4.Data.Repositories
{
    public class DokumentRepository : ControllerBase, IDokumentRepository
    {
        private IHostingEnvironment _env;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Anvandare> _usermanager;


        public DokumentRepository(IHostingEnvironment env, ApplicationDbContext dbContext, UserManager<Anvandare> userManager)
        {
            _env = env;
            _dbContext = dbContext;
            _usermanager = userManager;
        }

        public async Task<Dokument> Create(Dokument item)
        {
            //Either use a viewmodel or remove photoURL. URL will
            //be generated programmatically
             ModelState.Remove(nameof(Dokument.Path));
            item.Id = 0; //SQL Server will generate a new ID
            IFormFile file = item.File;
            //Check file extension 
            string extension =
                   Path.GetExtension(file.FileName);
            if (extension == ".png" || extension == ".jpg" || extension == ".txt" || extension == ".pdf")
            {
                //generate unique name to retrieve later
                //string newFileName = Guid.NewGuid().ToString();
                string newFileName = item.Namn;
                ////

                //store photo on file system and reference in DB
                if (file.Length > 0) //ensure the file is not empty
                {
                    string filePath = Path.Combine(_env.WebRootPath, "Uploads"
                                                , newFileName + extension);
                    //save location to database (in URL format)
                    item.Path = "Uploads/" + newFileName + extension;


                    item.Namn = newFileName + extension;
                    //write file to file system
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fs);
                    }
                    _dbContext.Dokument.Add(item);
                    _dbContext.SaveChanges();
                }

            }
            return item;
        }

        public async Task<bool> SaveAsync()
        {
            return (await _dbContext.SaveChangesAsync()) >= 0;
        }

        public FileResult DownloadFile(string filename)
        {
            string path = Path.Combine(this._env.WebRootPath, "Uploads/") + filename;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return  File(bytes, "application/octet-stream", filename);
        }

        public List<Dokument> GetAllDokument()
        {
            string[] filespaths = Directory.GetFiles(Path.Combine(this._env.WebRootPath, "Uploads/"));
            List<Dokument> list = new List<Dokument>();
            foreach (string filepath in filespaths)
            {
                list.Add(new Dokument { Namn = Path.GetFileName(filepath) });


            }
            return list;
        }

        public async Task<Dokument> UploadDokumentKurs(Dokument dokument, int? kursId, int? dokumentTypId)
        {
            //Either use a viewmodel or remove photoURL. URL will
            //be generated programmatically
            // ModelState.Remove(nameof(Dokument.Path));
            dokument.Id = 0; //SQL Server will generate a new ID

            //if (ModelState.IsValid)
            //{
            IFormFile file = dokument.File;
            //Check file extension 
            string extension =
                   Path.GetExtension(file.FileName);
            if (extension == ".png" || extension == ".jpg" || extension == ".txt")
            {
                //TODO: Use ImageSharp to resize uploaded photo
                //https://www.hanselman.com/blog/HowDoYouUseSystemDrawingInNETCore.aspx

                //generate unique name to retrieve later
                //string newFileName = Guid.NewGuid().ToString();
                string newFileName = dokument.Namn;
                dokument.KursId = kursId;
                dokument.Anvandare = await _usermanager.GetUserAsync(User);

                
                //store photo on file system and reference in DB
                if (file.Length > 0) //ensure the file is not empty
                {
                    string filePath = Path.Combine(_env.WebRootPath, "Uploads"
                                                , newFileName + extension);
                    //save location to database (in URL format)
                    dokument.Path = "Uploads/" + newFileName + extension;

                    //write file to file system
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fs);

                    }
                    _dbContext.Dokument.Add(dokument);
                    //_dbContext.SaveChanges();
                    // return RedirectToAction("Index", "Home");
                }

            }
            return dokument;
        }

        public Task<Dokument> UploadDokumentModul(Dokument input, int kursId, int modulId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dokument> UploadDokument(Dokument dokument, int? dokumentTypId)
        {
            //Either use a viewmodel or remove photoURL. URL will
            //be generated programmatically
            // ModelState.Remove(nameof(Dokument.Path));
            dokument.Id = 0; //SQL Server will generate a new ID

            //if (ModelState.IsValid)
            //{
            IFormFile file = dokument.File;
            //Check file extension 
            string extension =
                   Path.GetExtension(file.FileName);
            if (extension == ".png" || extension == ".jpg" || extension == ".txt")
            {
                //TODO: Use ImageSharp to resize uploaded photo
                //https://www.hanselman.com/blog/HowDoYouUseSystemDrawingInNETCore.aspx

                //generate unique name to retrieve later
                //string newFileName = Guid.NewGuid().ToString();
                string newFileName = dokument.Namn;
                //dokument.DokumentTypId = dokumentTypId;
                //dokument.Anvandare = await _usermanager.GetUserAsync(User);


                //store photo on file system and reference in DB
                if (file.Length > 0) //ensure the file is not empty
                {
                    string filePath = Path.Combine(_env.WebRootPath, "Uploads"
                                                , newFileName + extension);
                    //save location to database (in URL format)
                    dokument.Path = "Uploads/" + newFileName + extension;

                    //write file to file system
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fs);

                    }
                    _dbContext.Dokument.Add(dokument);
                    //_dbContext.SaveChanges();
                    // return RedirectToAction("Index", "Home");
                }

            }
            return dokument;
        }
    }
}


