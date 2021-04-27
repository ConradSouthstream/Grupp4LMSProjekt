using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Grupp4.Web.Controllers
{
    public class AktivitetController : BaseController
    {
        // GET: AktivitetController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AktivitetController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AktivitetController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AktivitetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AktivitetController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AktivitetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AktivitetController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AktivitetController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
