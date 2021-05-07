﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Data;
using LMS.Grupp4.Core.ViewModels.DokumentViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using LMS.Grupp4.Core.IRepository;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace LMS.Grupp4.Web.Controllers
{
    [Authorize(Roles = "Lärare")]
    public class KurserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly UserManager<Anvandare> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;



        public KurserController(ApplicationDbContext context, IMapper mapper, IUnitOfWork uow, UserManager<Anvandare> usermanager, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _uow = uow;
            _userManager = usermanager;
            _env = env;
        }

        // GET: Kurs
        public async Task<IActionResult> Index()
        {
            var kurs = await _context.Kurser.Include(k => k.Moduler).ToListAsync();
            return View(kurs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurs = await _context.Kurser
                .Include(c => c.AnvandareKurser)
                .ThenInclude(e => e.Anvandare)
                .Include(c => c.Moduler)
                .FirstOrDefaultAsync(m => m.Id == id);
            var dokument = await _context.Dokument
                .Where(d => d.KursId == kurs.Id).ToListAsync();

            kurs.Dokument = dokument;
            if (kurs == null)
            {
                return NotFound();
            }

            return View(kurs);
        }

        private IEnumerable<SelectListItem> GetDokumentTypNamn()

        {
            var TypeName = _context.DokumentTyper;

            var GetTypNamn = new List<SelectListItem>();
            foreach (var type in TypeName)
            {
                var newNamn = (new SelectListItem
                {
                    Text = type.Namn,
                    Value = type.Id.ToString(),
                });
                GetTypNamn.Add(newNamn);
            }
            return (GetTypNamn);
        }

        public FileResult DownloadFile(string filename)
        {
            string path = Path.Combine(_env.WebRootPath, "Uploads/") + filename;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", filename);
        }

        public IActionResult Upload(int id)
        {
            var Dokument = new Dokument
            {
                GetDokumentTypNamn = GetDokumentTypNamn(),
                KursId = id
            };
            return View(Dokument);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(Dokument upload)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            upload.Anvandare = await _userManager.GetUserAsync(User);

            await _uow.DokumentRepository.Create(upload);

            await _uow.CompleteAsync();

            TempData["msg"] = "Filen har laddats upp";
            return Redirect("/Kurser/Details/" + upload.KursId);

        }

        // GET: Kurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Namn,StartDatum,SlutDatum,Beskrivning")] Kurs kurs)
        {
            if (ModelState.IsValid)
            {
                if (kurs.SlutDatum < DateTime.Now && kurs.StartDatum < DateTime.Now)
                {
                    kurs.KursStatus = Status.Avslutad;
                }
                if (kurs.StartDatum <= DateTime.Now && kurs.SlutDatum.AddHours(23) >= DateTime.Now)
                {
                    kurs.KursStatus = Status.Aktuell;
                }
                if (kurs.StartDatum > DateTime.Now.AddDays(1) && kurs.SlutDatum > DateTime.Now)
                {
                    kurs.KursStatus = Status.Kommande;
                }

                _context.Add(kurs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kurs);
        }

        // GET: Kurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurs = await _context.Kurser.FindAsync(id);
            if (kurs == null)
            {
                return NotFound();
            }
            return View(kurs);
        }

        // POST: Kurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Namn,StartDatum,SlutDatum,Beskrivning")] Kurs kurs)
        {
            if (id != kurs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kurs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KursExists(kurs.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kurs);
        }

        // GET: Kurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurs = await _context.Kurser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kurs == null)
            {
                return NotFound();
            }

            return View(kurs);
        }

        // POST: Kurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kurs = await _context.Kurser.FindAsync(id);
            _context.Kurser.Remove(kurs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KursExists(int id)
        {
            return _context.Kurser.Any(e => e.Id == id);
        }
    }
}
