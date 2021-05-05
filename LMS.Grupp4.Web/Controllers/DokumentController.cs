using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Core.IRepository;
using LMS.Grupp4.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    public class DokumentController : Controller
    {
        private readonly IUnitOfWork uow;
        public DokumentController(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(DocumentInput upload)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var result = await uow.DokumentRepository.Create(upload);

            return View(result);
        }

    }
}
