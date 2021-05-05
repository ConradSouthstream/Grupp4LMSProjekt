using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Grupp4.Data.Repositories
{
    public class DokumentRepository : IDokumentRepository
    {
        private readonly ApplicationDbContext m_DbContext;
        private IHostingEnvironment _hostingEnvironment;
        public DokumentRepository(ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            m_DbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public  async Task<Dokument> Create(DocumentInput input)
        {
            var file = input.File;
            //var parsedContentDisposition =
            //    ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            string extension =
                       Path.GetExtension(input.File.FileName);

            // var parsedFilename = HeaderUtilities.RemoveQuotes(parsedContentDisposition.FileName);
            var filename = Guid.NewGuid().ToString();// Path.GetExtension(parsedFilename);


            var fileDestination = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", filename+extension);


            var upload = new Dokument()
            {
               
                Namn = input.Name,               
                Path = fileDestination
                
            };

            using (var fileStream = new FileStream(fileDestination, FileMode.Create))
            {
                var inputStream = file.OpenReadStream();
                await inputStream.CopyToAsync(fileStream);
            }

            m_DbContext.Dokument.Add(upload);
            await m_DbContext.SaveChangesAsync();

            return upload;
        }
    }
}
