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
    public class DokumentTypController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DokumentTypController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DokumentTyp
        public async Task<IActionResult> Index()
        {
            return View(await _context.DokumentTyper.ToListAsync());
        }

        // GET: DokumentTyp/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dokumentTyp = await _context.DokumentTyper
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dokumentTyp == null)
            {
                return NotFound();
            }

            return View(dokumentTyp);
        }

        // GET: DokumentTyp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DokumentTyp/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Namn")] DokumentTyp dokumentTyp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dokumentTyp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dokumentTyp);
        }

        // GET: DokumentTyp/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dokumentTyp = await _context.DokumentTyper.FindAsync(id);
            if (dokumentTyp == null)
            {
                return NotFound();
            }
            return View(dokumentTyp);
        }

        // POST: DokumentTyp/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Namn")] DokumentTyp dokumentTyp)
        {
            if (id != dokumentTyp.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dokumentTyp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DokumentTypExists(dokumentTyp.Id))
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
            return View(dokumentTyp);
        }

        // GET: DokumentTyp/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dokumentTyp = await _context.DokumentTyper
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dokumentTyp == null)
            {
                return NotFound();
            }

            return View(dokumentTyp);
        }

        // POST: DokumentTyp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dokumentTyp = await _context.DokumentTyper.FindAsync(id);
            _context.DokumentTyper.Remove(dokumentTyp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DokumentTypExists(int id)
        {
            return _context.DokumentTyper.Any(e => e.Id == id);
        }
    }
}
