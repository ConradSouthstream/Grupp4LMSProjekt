using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Data;
using Microsoft.AspNetCore.Authorization;

namespace LMS.Grupp4.Web.Controllers
{
    [Authorize(Roles = "Lärare")]
    public class KurserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KurserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Kurs
        public async Task<IActionResult> Index()
        {
            var kurs =await _context.Kurser.Include(k => k.Moduler).ToListAsync();
            return View(kurs);
        }

        // GET: Kurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurs = await _context.Kurser
                 .Include(c => c.AnvandareKurser)
                .ThenInclude(e => e.Anvandare)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kurs == null)
            {
                return NotFound();
            }

            return View(kurs);
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
