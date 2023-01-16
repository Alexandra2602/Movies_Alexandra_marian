using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies_Alexandra_marian.Data;
using Movies_Alexandra_marian.Models;
using Microsoft.AspNetCore.Authorization;

namespace Movies_Alexandra_marian.Controllers
{
    [Authorize(Roles = "Employee")]
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var books = from b in _context.Movies
                        join a in _context.Directors on b.DirectorID equals a.DirectorID
                        select new Movie
                        {
                            ID = b.ID,
                            Title = b.Title,
                            Director = a
                        };


            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Movie>.CreateAsync(books, pageNumber ?? 1, pageSize));
        }

        // GET: Movies/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(d => d.Director)
                .Include(h =>h.Histories)
                .ThenInclude(c=>c.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["LastName"] = new SelectList(_context.Directors, "DirectorID", "LastName");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,DirectorID")] Movie movie)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _context.Add(movie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }
            // ViewData["DirectorID"] = new SelectList(_context.Directors, "DirectorID", "DirectorID", movie.DirectorID);
            return View(movie);
        }
        
        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["LastName"] = new SelectList(_context.Directors, "DirectorID", "LastName", movie.DirectorID);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieToUpdate = await _context.Movies
                .Include(d=>d.Director)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (await TryUpdateModelAsync<Movie>(
                movieToUpdate,
                "",
                d => d.Director, s => s.Title))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(movieToUpdate);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .AsNoTracking()
                .Include(b => b.Director)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (movie == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Deleted failed.Try again";
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool MovieExists(int id)
        {
          return _context.Movies.Any(e => e.ID == id);
        }
    }
}
