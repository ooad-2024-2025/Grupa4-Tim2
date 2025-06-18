using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SarajevoGuide.Data;
using SarajevoGuide.Models;
using System.Security.Claims;
using System.Net.Mail;
using System.Net;

namespace SarajevoGuide.Controllers
{
    [Authorize]
    public class KupovinasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KupovinasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Kupovinas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kupovina.ToListAsync());
        }

        // GET: Kupovinas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kupovina = await _context.Kupovina
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kupovina == null)
            {
                return NotFound();
            }

            return View(kupovina);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] int brojUlaznica, [FromForm] int eventId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
                return RedirectToAction("Index", "RegistrovaniKorisniks");

            var korisnik = _context.RegistrovaniKorisnik
                .FirstOrDefault(k => k.email == userEmail);

            if (korisnik == null)
                return BadRequest("No RegistrovaniKorisnik found with your email.");

            var ev = await _context.Event.FindAsync(eventId);
            if (ev == null)
                return BadRequest("Event not found.");

            var kupovina = new Kupovina(0, DateTime.Now, brojUlaznica, korisnik.id, eventId);
            _context.Kupovina.Add(kupovina);
            await _context.SaveChangesAsync();

            var ukupnaCijena = brojUlaznica * ev.Price;
            var body = $@"
        <h2>Potvrda kupovine</h2>
        <p>Hvala na kupovini {brojUlaznica} ulaznica za <strong>{ev.Name}</strong>.</p>
        <p>Ukupna cijena: <strong>{ukupnaCijena} KM</strong></p>
        <p>Datum: {DateTime.Now:dd.MM.yyyy HH:mm}</p>";

            try
            {
                using var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("hhodzic550@gmail.com", "zzynbzamljuspzyx"),
                    EnableSsl = true
                };

                var message = new MailMessage("hhodzic550@gmail.com", userEmail)
                {
                    Subject = "Potvrda kupovine",
                    Body = body,
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Kupovina uspješna, ali email nije poslan.";
                Console.WriteLine("Greška prilikom slanja emaila:");
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("InnerException: " + ex.InnerException?.Message);
                Console.WriteLine("Full error: " + ex.ToString());
            }

            TempData["Success"] = "Kupovina uspješna. Potvrda poslana na email.";
            return RedirectToAction("Index", "Home");
        }





        // GET: Kupovinas/Create
        public IActionResult Create(string? eventName, int? eventId)
        {
            ViewData["EventName"] = eventName;
            ViewData["EventId"] = eventId;
            return View();
        }



        // POST: Kupovinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        // GET: Kupovinas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kupovina = await _context.Kupovina.FindAsync(id);
            if (kupovina == null)
            {
                return NotFound();
            }
            return View(kupovina);
        }

        // POST: Kupovinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      

        // GET: Kupovinas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kupovina = await _context.Kupovina
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kupovina == null)
            {
                return NotFound();
            }

            return View(kupovina);
        }

        // POST: Kupovinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kupovina = await _context.Kupovina.FindAsync(id);
            if (kupovina != null)
            {
                _context.Kupovina.Remove(kupovina);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KupovinaExists(int id)
        {
            return _context.Kupovina.Any(e => e.Id == id);
        }
    }
}
