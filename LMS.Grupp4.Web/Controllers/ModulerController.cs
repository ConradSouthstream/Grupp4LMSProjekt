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

namespace LMS.Grupp4.Web.Controllers
{
    public class ModulerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ModulerController(ApplicationDbContext context, IUnitOfWork uow, IMapper mapper)
        {
            _context = context;
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: Moduler
        public async Task<IActionResult> Index()
        {
            var moduler = _context.Moduler.Include(m => m.Kurs);
            return View(await moduler.ToListAsync());
        }

        // GET: Moduler/Details/5
        public async Task<IActionResult> Details(int? id)
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

       // GET: Moduler/Create
        public IActionResult Create()
        {
            var model = new Modul
            {
                GetKursNamn = GetKursNamn(),

            };
            return View(model);
        }

        //// POST: Moduler/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Namn,StartDatum,SlutDatum,Beskrivning,KursId")] Modul modul)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modul);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modul);
        }
        //public async Task<ActionResult> Create(int? id)
        //{
        //    if (id.HasValue)
        //    {
        //        ModulCreateViewModel viewModel = new ModulCreateViewModel();

        //        // Hämta information om Kurs
        //        Kurs kurs = await uow.KursRepository.GetKursAsync(id.Value);
        //        if (kurs != null)
        //        {
        //            viewModel.KursId = kurs.Id;

        //        }

        //        // Sätt upp startvärden för kalendrar
        //        DateTime dtNow = DateTime.Now;
        //        viewModel.StartDatum = dtNow;
        //        viewModel.SlutDatum = dtNow.AddDays(1);

        //        // Hämta alla AktivitetTyp från repository. Skapa en dropdown med AktivitetTyper
        //        List<Kurs> kurser = await uow.KursRepository.GetAllKurserAsync();
        //        List<SelectListItem> kursNamnDropDown = ModulHelper.CreateKursNamnDropDown(kurser, viewModel.KursId.ToString());
        //        viewModel.KursId = kursNamnDropDown;

        //        return View(viewModel);
        //    }

        //    ViewBag.Message = "Det gick inte gå till sidan för att skapa Modul";
        //    ViewBag.TypeOfMessage = (int)TypeOfMessage.Error;

        //    return RedirectToAction(nameof(Index));
        //}

        //// POST: AktivitetController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(ModulCreateViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            Modul modul= mapper.Map<Modul>(viewModel);

        //            // Post
        //            // https://www.c-sharpcorner.com/article/http-get-put-post-and-delete-verbs-in-asp-net-web-api/
        //            // Read = Get
        //            // Update = Put
        //            // Create = Post
        //            // Delete = Delete

        //            await uow.ModulRepository.AddModul(modul);
        //            if (await m_UnitOfWork.AktivitetRepository.SaveAsync())
        //            {// Vi har sparat en ny aktivitet. Redirect till listning

        //                TempData["message"] = $"Har skapat aktivitet {viewModel.Namn}";
        //                TempData["typeOfMessage"] = TypeOfMessage.Info;

        //                return RedirectToAction(nameof(Index));
        //            }
        //        }
        //        catch (Exception)
        //        { }
        //    }

        //    // Kommer vi hit har något gått fel
        //    ViewBag.Message = "Det gick inte skapa aktiviteten";
        //    ViewBag.TypeOfMessage = TypeOfMessage.Error;

        //    // Vi måste uppdatera viss data om modulen som inte view bindar till modellen
        //    Modul modul = await m_UnitOfWork.ModulRepository.GetModulAsync(viewModel.ModulId);
        //    if (modul != null)
        //    {
        //        viewModel.ModulId = modul.Id;
        //        viewModel.ModulNamn = modul.Namn;
        //        viewModel.ModulSlutDatum = modul.SlutDatum;
        //        viewModel.ModulStartDatum = modul.StartDatum;
        //    }

        //    // Hämta alla AktivitetTyp från repository. Skapa en dropdown med AktivitetTyper
        //    List<AktivitetTyp> lsAktivitetTyper = await m_UnitOfWork.AktivitetRepository.GetAktivitetTyperAsync();
        //    List<SelectListItem> lsAktivitetTyperDropDown = AktivitetHelper.CreateAktivitetTypDropDown(lsAktivitetTyper, viewModel.AktivitetTypId.ToString());
        //    viewModel.AktivitetTyper = lsAktivitetTyperDropDown;

        //    return View(viewModel);
        //}


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
            if (modul == null)
            {
                return NotFound();
            }
            ViewData["KursId"] = new SelectList(_context.Kurser, "Id", "Id", modul.KursId);
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
                return RedirectToAction(nameof(Index));
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
