﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SarajevoGuide.Data;
using SarajevoGuide.Models;

namespace SarajevoGuide.Controllers
{
    public class RecenzijasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecenzijasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recenzija
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recenzija.ToListAsync());
        }

        // GET: Recenzija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recenzija = await _context.Recenzija
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recenzija == null)
            {
                return NotFound();
            }

            return View(recenzija);
        }

        // GET: Recenzija/Create
        public IActionResult Create(int eventID)
        {
            Recenzija recenzija = new Recenzija();
            recenzija.EventId = eventID;
            return View(recenzija);
        }

        // POST: Recenzija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KorisnikId,EventId,Komentar,Ocjena")] Recenzija recenzija)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var korisnik = _context.RegistrovaniKorisnik.FirstOrDefault(x => x.email == email);
            if (korisnik == null) return BadRequest();

            recenzija.KorisnikId = korisnik.id;

            if (!ModelState.IsValid)
            {
                return View(recenzija); // prikazuje formu ponovo s validacijom
            }

            _context.Recenzija.Add(recenzija);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home"); // redirekcija nakon uspjeha
        }

        public IActionResult Reviews()
        {
            var recenzije = _context.Recenzija.ToList();
            var korisnici = _context.RegistrovaniKorisnik.ToDictionary(k => k.id, k => k.username);

            var model = recenzije.Select(r => new
            {
                Username = korisnici.TryGetValue(r.KorisnikId, out var username) ? username : "Nepoznat",
                Ocjena = r.Ocjena,
                Komentar = r.Komentar
            }).ToList();

            return View(model);
        }



        // GET: Recenzijas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recenzija = await _context.Recenzija.FindAsync(id);
            if (recenzija == null)
            {
                return NotFound();
            }
            return View(recenzija);
        }

        // POST: Recenzijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,korisnikId,eventId,komentar,ocjena")] Recenzija recenzija)
        {
            if (id != recenzija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recenzija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecenzijaExists(recenzija.Id))
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
            return View(recenzija);
        }

        // GET: Recenzijas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recenzija = await _context.Recenzija
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recenzija == null)
            {
                return NotFound();
            }

            return View(recenzija);
        }

        // POST: Recenzijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recenzija = await _context.Recenzija.FindAsync(id);
            if (recenzija != null)
            {
                _context.Recenzija.Remove(recenzija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecenzijaExists(int id)
        {
            return _context.Recenzija.Any(e => e.Id == id);
        }
    }
}
