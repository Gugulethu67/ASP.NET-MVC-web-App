using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Data;
using Expense_Tracker.Models;

namespace Expense_Tracker.Controllers
{
    public class TransationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transation.Include(t => t.Division);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transation == null)
            {
                return NotFound();
            }

            var transation = await _context.Transation
                .Include(t => t.Division)
                .FirstOrDefaultAsync(m => m.TransationId == id);
            if (transation == null)
            {
                return NotFound();
            }

            return View(transation);
        }

        // GET: Transations/Create
        public IActionResult Create(int id =0)
        {
            populateDivisions();
            if (id == 0)
                return View(new Transation());
            else
                return View(_context.Division.Find(id));
        }

        // POST: Transations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransationId,DivisionId,note,Amount,Date")] Transation transation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            populateDivisions();
            return View(transation);
        }

        // GET: Transations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transation == null)
            {
                return NotFound();
            }

            var transation = await _context.Transation.FindAsync(id);
            if (transation == null)
            {
                return NotFound();
            }
            ViewData["DivisionId"] = new SelectList(_context.Division, "DivisionId", "DivisionId", transation.DivisionId);
            return View(transation);
        }

        // POST: Transations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransationId,DivisionId,note,Amount,Date")] Transation transation)
        {
            if (id != transation.TransationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransationExists(transation.TransationId))
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
            ViewData["DivisionId"] = new SelectList(_context.Division, "DivisionId", "DivisionId", transation.DivisionId);
            return View(transation);
        }

        // GET: Transations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transation == null)
            {
                return NotFound();
            }

            var transation = await _context.Transation
                .Include(t => t.Division)
                .FirstOrDefaultAsync(m => m.TransationId == id);
            if (transation == null)
            {
                return NotFound();
            }

            return View(transation);
        }

        // POST: Transations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Transation'  is null.");
            }
            var transation = await _context.Transation.FindAsync(id);
            if (transation != null)
            {
                _context.Transation.Remove(transation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransationExists(int id)
        {
          return (_context.Transation?.Any(e => e.TransationId == id)).GetValueOrDefault();
        }

        [NonAction]
        public void populateDivisions()
        {
            var divisionCollection = _context.Division.ToList();
            Division defaultDivision = new Division() { DivisionId = 0, Name = "Choose the Division" };
            divisionCollection.Insert(0, defaultDivision);
            ViewBag.Division = divisionCollection;
        }
    }
}
