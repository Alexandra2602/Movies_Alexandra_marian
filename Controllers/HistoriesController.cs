using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies_Alexandra_marian.Data;
using Movies_Alexandra_marian.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Movies_Alexandra_marian.Controllers
{
    public class HistoriesController : Controller
    {
        private readonly MovieContext _context;

        public HistoriesController(MovieContext context)
        {
            _context = context;
        }

        // GET: Histories
        public async Task<IActionResult> Index()
        {
            var movieContext = _context.Histories.Include(h => h.Customer).Include(h => h.Movie);
            return View(await movieContext.ToListAsync());
        }

        // GET: Histories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Histories == null)
            {
                return NotFound();
            }

            var history = await _context.Histories
                .Include(h => h.Customer)
                .Include(h => h.Movie)
                .FirstOrDefaultAsync(m => m.HistoryID == id);
            if (history == null)
            {
                return NotFound();
            }

            return View(history);
        }

        // GET: Histories/Create
        public IActionResult Create()
        {
            ViewData["CustomerName"] = new SelectList(_context.Customers, "CustomerID", "Name");
            ViewData["MovieTitle"] = new SelectList(_context.Movies, "ID", "Title");
            return View();
        }

        // POST: Histories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HistoryID,CustomerID,MovieID,OrderDate")] History history)
        {
            if (ModelState.IsValid)
            {
                _context.Add(history);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerName"] = new SelectList(_context.Customers, "CustomerID", "Name", history.CustomerID);
            ViewData["MovieTitle"] = new SelectList(_context.Movies, "ID", "Title", history.MovieID);
            return View(history);
        }

        // GET: Histories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Histories == null)
            {
                return NotFound();
            }

            var history = await _context.Histories.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }
            ViewData["CustomerName"] = new SelectList(_context.Customers, "CustomerID", "Name", history.CustomerID);
            ViewData["MovieTitle"] = new SelectList(_context.Movies, "ID", "Title", history.MovieID);
            return View(history);
        }

        // POST: Histories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HistoryID,CustomerID,MovieID,OrderDate")] History history)
        {
            if (id != history.HistoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(history);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoryExists(history.HistoryID))
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
            ViewData["CustomerName"] = new SelectList(_context.Customers, "CustomerID", "Name", history.CustomerID);
            ViewData["MovieTitle"] = new SelectList(_context.Movies, "ID", "Title", history.MovieID);
            return View(history);
        }

        // GET: Histories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Histories == null)
            {
                return NotFound();
            }

            var history = await _context.Histories
                .Include(h => h.Customer)
                .Include(h => h.Movie)
                .FirstOrDefaultAsync(m => m.HistoryID == id);
            if (history == null)
            {
                return NotFound();
            }

            return View(history);
        }

        // POST: Histories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Histories == null)
            {
                return Problem("Entity set 'MovieContext.Histories'  is null.");
            }
            var history = await _context.Histories.FindAsync(id);
            if (history != null)
            {
                _context.Histories.Remove(history);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoryExists(int id)
        {
          return _context.Histories.Any(e => e.HistoryID == id);
        }
    }
}
