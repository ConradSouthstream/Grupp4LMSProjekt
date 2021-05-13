using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Data;
using LMS.Grupp4.Core.IRepository;
using AutoMapper;
using LMS.Grupp4.Core.ViewModels.Modul;
using LMS.Grupp4.Web.Utils;
using LMS.Grupp4.Core.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;


namespace LMS.Grupp4.Web.Controllers
{
    public class ModulerController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<Anvandare> _userManager;


        public ModulerController(ApplicationDbContext context, IUnitOfWork uow, IMapper mapper, IWebHostEnvironment env, UserManager<Anvandare> userManager): 
            base(uow, mapper, userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        public IActionResult Upload(int id)
        {
            var Dokument = new Dokument
            {
                GetDokumentTypNamn = GetDokumentTypNamn(),
                ModulId = id
               
            };
            return View(Dokument);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(Dokument upload)
        {
            //if (!ModelState.IsValid)
            //{
            //    return NotFound();
            //}
            upload.Anvandare = await _userManager.GetUserAsync(User);

            // var dokument = m_Mapper.Map<Dokument>(upload);


            await m_UnitOfWork.DokumentRepository.Create(upload);

            await m_UnitOfWork.CompleteAsync();

            TempData["msg"] = "Filen har laddats upp";
            //return Redirect("/Elev/Details/"+ dokument.KursId);
            return Redirect("/Moduler/Details/" + upload.ModulId);

            // return View(dokument);
        }

        // GET: Moduler
        public async Task<IActionResult> Index()
        {
            var moduler = _context.Moduler.Include(m => m.Kurs).Include(m=>m.Dokument);
            return View(await moduler.ToListAsync());
        }

        // GET: Moduler/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{

        //    var modul = await _context.Moduler
        //                    .Include(c => c.Aktiviteter)
        //                    .FirstOrDefaultAsync(m => m.Id == id);
        //    var dokument = await _context.Dokument
        //        .Where(d => d.ModulId == modul.Id).ToListAsync();

        //    modul.Dokument = dokument;

        //   // var modul = await mapper
        //   //     .ProjectTo<ModulDetaljerViewModel>(_context.Moduler)
        //   //     .FirstOrDefaultAsync(s => s.Id == id); 
        //   // var dokument = await _context.Dokument
        //   //.Where(d => d.ModulId == modul.Id).ToListAsync();
        //   // modul.Dokument = dokument;
        //    return View(modul);
        //}
        public async Task<IActionResult> Details(int? id)
        {
            GetMessageFromTempData();

            if (id == null)
            {
                return NotFound();
            }
            var modul = await _context.Moduler.Include(m=>m.Kurs)
                .Include(c => c.Aktiviteter)            
                .FirstOrDefaultAsync(m => m.Id == id);

            var kurs = _context.Kurser.Where(k => k.Id == modul.KursId).FirstOrDefault();
            var dokument = await _context.Dokument.Include(d=>d.Anvandare)
                .Where(d => d.ModulId == modul.Id).ToListAsync();
            modul.KursId = kurs.Id;                
            modul.Dokument = dokument;
            if (modul == null)
            {
                return NotFound();
            }

            return View(modul);
        }

        // GET: Moduler/Create
        public IActionResult Create(int? Id)
        {
            var kurs = _context.Kurser.Include(kurs => kurs.Moduler).Where(k => k.Id == Id).FirstOrDefault();
            
            //Kolla om det finns aktiviteter på kursen, om det gör det ta det äldsta slutdatumet som startdatum.
            //Annars ta kursens startdatum.
            DateTime SenasteModulDatum;
            if (kurs.Moduler.Any())
            { SenasteModulDatum = kurs.Moduler.Max(m => m.SlutDatum); }
            else { SenasteModulDatum = kurs.StartDatum; }

            var model = new Modul
            {   
                StartDatum = SenasteModulDatum.AddDays(1),
                SlutDatum = SenasteModulDatum.AddDays(6),
                GetKursNamn = GetKursNamn(),                
                Kurs = kurs,
                KursId = kurs.Id,
            };

            return View(model);
        }

        //// POST: Moduler/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Namn, StartDatum, SlutDatum, Beskrivning, KursId")] Modul viewModel)
        {
            if (ModelState.IsValid)
            {
                var modul = viewModel;
                _context.Add(modul);
                await _context.SaveChangesAsync();

                TempData["message"] = $"Har skapat modul {viewModel.Namn}";
                TempData["typeOfMessage"] = TypeOfMessage.Info;
                return RedirectToAction(nameof(Details), "Kurser", new { Id = viewModel.KursId });
            }
            return View(viewModel);
        }

        //En lista på alla kursnamn och Id'n
        private IEnumerable<SelectListItem> GetKursNamn()

        {
            var TypeName = _context.Kurser;
            var GetKursNamn = new List<SelectListItem>();
            foreach (var type in TypeName)
            {
                var newNamn = (new SelectListItem
                {
                    Text = type.Namn,
                    Value = type.Id.ToString(),
                });
                GetKursNamn.Add(newNamn);
            }
            return (GetKursNamn);
        }

        // GET: Moduler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modul = await _context.Moduler.FindAsync(id);
            var kurs = await _context.Kurser.Include(kurs => kurs.Moduler).Where(k => k.Id == modul.KursId).FirstOrDefaultAsync();
            if (modul == null)
            {
                return NotFound();
            }
            modul.GetKursNamn = GetKursNamn();
            modul.Kurs = kurs;
            return View(modul);
        }

        // POST: Moduler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Namn,StartDatum,SlutDatum,Beskrivning,KursId")] Modul modul)
        {
            if (id != modul.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modul);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModulExists(modul.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["message"] = $"Har uppdaterat modul {modul.Namn}";
                TempData["typeOfMessage"] = TypeOfMessage.Info;
                return RedirectToAction(nameof(Details), "Kurser", new { Id = modul.KursId });
            }
            ViewData["KursId"] = new SelectList(_context.Kurser, "Id", "Id", modul.KursId);
            return View(modul);
        }

        // GET: Moduler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modul = await _context.Moduler
                .Include(m => m.Kurs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modul == null)
            {
                return NotFound();
            }

            return View(modul);
        }

        // POST: Moduler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modul = await _context.Moduler.FindAsync(id);
            _context.Moduler.Remove(modul);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModulExists(int id)
        {
            return _context.Moduler.Any(e => e.Id == id);
        }

    }
}
