using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Grupp4.Core.Entities;
using LMS.Grupp4.Data;

namespace LMS.Grupp4.Web.Controllers
{
    public class AnvandareKursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnvandareKursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AnvandareKurs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AnvandareKurser.Include(a => a.Anvandare).Include(a => a.Kurs);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AnvandareKurs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anvandareKurs = await _context.AnvandareKurser
                .Include(a => a.Anvandare)
                .Include(a => a.Kurs)
                .FirstOrDefaultAsync(m => m.AnvandareId == id);
            if (anvandareKurs == null)
            {
                return NotFound();
            }

            return View(anvandareKurs);
        }

        // GET: AnvandareKurs/Create
        public IActionResult Create()
        {
            ViewData["AnvandareId"] = new SelectList(_context.Anvandare, "Id", "Id");
            ViewData["KursId"] = new SelectList(_context.Kurser, "Id", "Id");
            return View();
        }

        // POST: AnvandareKurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Betyg,KursId,AnvandareId")] AnvandareKurs anvandareKurs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(anvandareKurs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnvandareId"] = new SelectList(_context.Anvandare, "Id", "Id", anvandareKurs.AnvandareId);
            ViewData["KursId"] = new SelectList(_context.Kurser, "Id", "Id", anvandareKurs.KursId);
            return View(anvandareKurs);
        }

        // GET: AnvandareKurs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anvandareKurs = await _context.AnvandareKurser.FindAsync(id);
            if (anvandareKurs == null)
            {
                return NotFound();
            }
            ViewData["AnvandareId"] = new SelectList(_context.Anvandare, "Id", "Id", anvandareKurs.AnvandareId);
            ViewData["KursId"] = new SelectList(_context.Kurser, "Id", "Id", anvandareKurs.KursId);
            return View(anvandareKurs);
        }

        // POST: AnvandareKurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Betyg,KursId,AnvandareId")] AnvandareKurs anvandareKurs)
        {
            if (id != anvandareKurs.AnvandareId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(anvandareKurs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnvandareKursExists(anvandareKurs.AnvandareId))
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
            ViewData["AnvandareId"] = new SelectList(_context.Anvandare, "Id", "Id", anvandareKurs.AnvandareId);
            ViewData["KursId"] = new SelectList(_context.Kurser, "Id", "Id", anvandareKurs.KursId);
            return View(anvandareKurs);
        }

        // GET: AnvandareKurs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anvandareKurs = await _context.AnvandareKurser
                .Include(a => a.Anvandare)
                .Include(a => a.Kurs)
                .FirstOrDefaultAsync(m => m.AnvandareId == id);
            if (anvandareKurs == null)
            {
                return NotFound();
            }

            return View(anvandareKurs);
        }

        // POST: AnvandareKurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var anvandareKurs = await _context.AnvandareKurser.FindAsync(id);
            _context.AnvandareKurser.Remove(anvandareKurs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnvandareKursExists(string id)
        {
            return _context.AnvandareKurser.Any(e => e.AnvandareId == id);
        }
    }
}
