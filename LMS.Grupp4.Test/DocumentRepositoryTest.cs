using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Data;
using LMS.Grupp4.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace LMS.Grupp4.Test
{
   public class DocumentRepositoryTest
    {
        private readonly UserManager<Anvandare> _usermanager;
        private IHostingEnvironment _env;
        private readonly ApplicationDbContext _dbContext;

        private static Mock<IFormFile> GetMockFormFileWithQuotedContentDisposition(string modelName, string filename)
            {
                var formFile = new Mock<IFormFile>();
                formFile.Setup(f => f.ContentDisposition)
                    .Returns(string.Format("form-data; name='{0}'; filename=\"{1}\"", modelName, filename));

                var ms = new MemoryStream();
                var writer = new StreamWriter(ms);
                writer.Write("Text content for the test file");
                writer.Flush();

                ms.Position = 0;

                formFile.Setup(m => m.OpenReadStream()).Returns(ms);

                return formFile;
            }

            private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
            {
            // Create a fresh service provider, and therefore a fresh 
            var serviceProvider = new ServiceCollection()
                    .BuildServiceProvider();

                // Create a new options instance telling the context to use an
                // sql database and the new service provider.
                var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
           
            builder.UseSqlServer(connectionString: "DefaultConnection")
                       .UseInternalServiceProvider(serviceProvider);

                return builder.Options;
            }


            [Ignore("ignore")]
            public async Task Uploads_Stored_In_Documents_Folder()
            {
                var contextOptions = CreateNewContextOptions();

                var fileMock = GetMockFormFileWithQuotedContentDisposition("UploadTest", "UploadTest.txt");

                var mockEnvironment = new Mock<IHostingEnvironment>();
                mockEnvironment.Setup(m => m.WebRootPath).Returns(Path.GetTempPath());
                mockEnvironment.Setup(m => m.ContentRootPath).Returns(Path.GetTempPath());

                using (var context = new ApplicationDbContext( contextOptions))
                {
                    // Ensure the path exists.
                    Directory.CreateDirectory(Path.GetTempPath() + "Uploads");

                    var sut = new DokumentRepository(_env,context, _usermanager);
                    var file = fileMock.Object;

                    var input = new Dokument()
                    {
                        Namn = "Upload Test",
                        File = fileMock.Object
                    };
                    //Act
                    var result = await sut.Create(input);

                    Assert.True(File.Exists(result.Path));
                    Assert.AreEqual(result.Namn, input.Namn);

                    // Clean up.
                    File.Delete(result.Path);

                }
            }
        }
    }

