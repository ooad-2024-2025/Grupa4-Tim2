using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SarajevoGuide.Data;
using SarajevoGuide.Models;
using SarajevoGuide.Enums;

namespace SarajevoGuide.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Event.Select(e => new
            {
                e.Id,
                name = e.Name,
                lat = e.Lat,
                lng = e.Lng,
                kategorija = e.Kategorija.ToString()
            }).ToListAsync();

            return Json(events);
        }


        // GET: Events
        public async Task<IActionResult> Index(string category)
        {
            var events = _context.Event.AsQueryable();

            if (!string.IsNullOrEmpty(category) && Enum.TryParse(category, out Kategorija parsedCategory))
            {
                events = events.Where(e => e.Kategorija == parsedCategory);
            }

            return View(await events.ToListAsync());
        }


        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            PopulateKategorijaDropdown();
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Kategorija,Description,StartDate,EndDate,Lat,Lng,Price")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();

                // Check if the request is from the modal (AJAX)
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Event created successfully!" });
                }

                return RedirectToAction(nameof(Index));
            }

            // If validation failed and it's an AJAX request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
                    .ToArray();
                return Json(new { success = false, errors = errors });
            }

            PopulateKategorijaDropdown(@event.Kategorija);
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            PopulateKategorijaDropdown(@event.Kategorija);
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Kategorija,Description,StartDate,EndDate,Lat,Lng,Price")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            PopulateKategorijaDropdown(@event.Kategorija);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            if (@event != null)
            {
                _context.Event.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }

        private void PopulateKategorijaDropdown(object selectedKategorija = null)
        {
            var kategorijaList = from Kategorija k in Enum.GetValues(typeof(Kategorija))
                                 select new { Value = (int)k, Text = k.ToString() };
            ViewBag.Kategorija = new SelectList(kategorijaList, "Value", "Text", selectedKategorija);
        }
    }
}