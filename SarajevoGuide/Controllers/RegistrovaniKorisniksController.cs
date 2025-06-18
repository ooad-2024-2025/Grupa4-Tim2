using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SarajevoGuide.Data;
using SarajevoGuide.Models;

namespace SarajevoGuide.Controllers
{
    public class RegistrovaniKorisniksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;


        public RegistrovaniKorisniksController(
            ApplicationDbContext context,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: RegistrovaniKorisniks
        public async Task<IActionResult> Index()
        {
            return View(await _context.RegistrovaniKorisnik.ToListAsync());
        }

        // GET: RegistrovaniKorisniks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrovaniKorisnik = await _context.RegistrovaniKorisnik
                .FirstOrDefaultAsync(m => m.id == id);
            if (registrovaniKorisnik == null)
            {
                return NotFound();
            }

            return View(registrovaniKorisnik);
        }

        // GET: RegistrovaniKorisniks/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: RegistrovaniKorisniks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: RegistrovaniKorisniks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,ime,prezime,email,lozinka,username")] RegistrovaniKorisnik registrovaniKorisnik)
        {
            if (!ModelState.IsValid)
                return View(registrovaniKorisnik);

            // Check if email is already used
            var existingUser = await _userManager.FindByEmailAsync(registrovaniKorisnik.email);
            if (existingUser != null)
            {
                ModelState.AddModelError("email", "Email is already taken.");
                return View(registrovaniKorisnik);
            }

            var identityUser = new IdentityUser
            {
                UserName = registrovaniKorisnik.username,
                Email = registrovaniKorisnik.email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, registrovaniKorisnik.lozinka);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUser, "User");
                _context.Add(registrovaniKorisnik);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(registrovaniKorisnik);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("email,lozinka")] RegistrovaniKorisnik model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Special case for Admin login
            if (model.email == "admin@example.com" && model.lozinka == "Admin123!")
            {
                var adminUser = await _userManager.FindByEmailAsync(model.email);
                if (adminUser != null)
                {
                    await _signInManager.SignInAsync(adminUser, isPersistent: false);
                    return RedirectToAction("Index", "Events");
                }
                else
                {
                    ModelState.AddModelError("", "Admin account is not set up.");
                    return View(model);
                }
            }

            // Check if the custom user exists in your table
            var korisnik = _context.RegistrovaniKorisnik
                .FirstOrDefault(u => u.email == model.email && u.lozinka == model.lozinka);

            if (korisnik == null)
            {
                ModelState.AddModelError("", "Incorrect email or password. Please sign up first.");
                return View(model);
            }

            var identityUser = await _userManager.FindByEmailAsync(model.email);
            if (identityUser == null)
            {
                ModelState.AddModelError("", "User identity not found.");
                return View(model);
            }

            await _signInManager.SignInAsync(identityUser, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }




        // GET: RegistrovaniKorisniks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrovaniKorisnik = await _context.RegistrovaniKorisnik.FindAsync(id);
            if (registrovaniKorisnik == null)
            {
                return NotFound();
            }
            return View(registrovaniKorisnik);
        }

        // POST: RegistrovaniKorisniks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ime,prezime,email,lozinka,username")] RegistrovaniKorisnik registrovaniKorisnik)
        {
            if (id != registrovaniKorisnik.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrovaniKorisnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrovaniKorisnikExists(registrovaniKorisnik.id))
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
            return View(registrovaniKorisnik);
        }

        // GET: RegistrovaniKorisniks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrovaniKorisnik = await _context.RegistrovaniKorisnik
                .FirstOrDefaultAsync(m => m.id == id);
            if (registrovaniKorisnik == null)
            {
                return NotFound();
            }

            return View(registrovaniKorisnik);
        }

                [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.RegistrovaniKorisnik
                .Select(u => new
                {
                    u.id,
                    u.ime,
                    u.prezime,
                    u.email,
                    u.username
                })
                .ToListAsync();

            return Json(users);
        }

        // POST: RegistrovaniKorisniks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registrovaniKorisnik = await _context.RegistrovaniKorisnik.FindAsync(id);
            if (registrovaniKorisnik != null)
            {
                _context.RegistrovaniKorisnik.Remove(registrovaniKorisnik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrovaniKorisnikExists(int id)
        {
            return _context.RegistrovaniKorisnik.Any(e => e.id == id);
        }
    }
}
