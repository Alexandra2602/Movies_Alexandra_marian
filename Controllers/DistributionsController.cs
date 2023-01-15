using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies_Alexandra_marian.Data;
using Movies_Alexandra_marian.Models;
using Movies_Alexandra_marian.Models.MovieViewModels;

namespace Movies_Alexandra_marian.Controllers
{
    [Authorize(Policy ="OnlyStaff")]
    public class DistributionsController : Controller
    {
        private readonly MovieContext _context;

        public DistributionsController(MovieContext context)
        {
            _context = context;
        }

        // GET: Distributions
        public async Task<IActionResult> Index(int? id, int? movieID)
        {
            var viewModel = new DistributionIndexData();
            viewModel.Distributions = await _context.Distributions
            .Include(i => i.DistributedMovies)
            .ThenInclude(i => i.Movie)
             .ThenInclude(i => i.Histories)
             .ThenInclude(i => i.Customer)
            
             .AsNoTracking()
             .OrderBy(i => i.DistributionName)
             .ToListAsync();

            if (id != null)
            {
                ViewData["DistributionID"] = id.Value;
                Distribution distribution = viewModel.Distributions.Where(
                i => i.ID == id.Value).Single();
                viewModel.Movies = distribution.DistributedMovies.Select(s => s.Movie);
            }
            if (movieID != null)
            {
                ViewData["MovieID"] = movieID.Value;
                viewModel.Histories = viewModel.Movies.Where(
                x => x.ID == movieID).Single().Histories;
            }
            return View(viewModel);
        }

        // GET: Distributions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Distributions == null)
            {
                return NotFound();
            }

            var distribution = await _context.Distributions
                .FirstOrDefaultAsync(m => m.ID == id);
            if (distribution == null)
            {
                return NotFound();
            }

            return View(distribution);
        }

        // GET: Distributions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Distributions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DistributionName,Adress")] Distribution distribution)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distribution);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(distribution);
        }

        // GET: Distributions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distribution = await _context.Distributions
              .Include(i => i.DistributedMovies).ThenInclude(i => i.Movie)
              .AsNoTracking()
              .FirstOrDefaultAsync(m => m.ID == id);

            if (distribution == null)
            {
                return NotFound();
            }
            PopulateDistributedMovieData(distribution);
            return View(distribution);
        }
        private void PopulateDistributedMovieData(Distribution distribution)
        {
            var allMovies = _context.Movies;
            var distributionMovies = new HashSet<int>(distribution.DistributedMovies.Select(c => c.MovieID));
            var viewModel = new List<DistributedMovieData>();
            foreach (var movie in allMovies)
            {
                viewModel.Add(new DistributedMovieData
                {
                    MovieID = movie.ID,
                    Title = movie.Title,
                    IsDistributed = distributionMovies.Contains(movie.ID)
                });
            }
            ViewData["Movies"] = viewModel;
        }
    

        // POST: Distributions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedMovies)
        {
            if (id == null)
            {
                return NotFound();
            }
            var distributionToUpdate = await _context.Distributions
            .Include(i => i.DistributedMovies)
            .ThenInclude(i => i.Movie)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Distribution>(
            distributionToUpdate,
            "",
            i => i.DistributionName, i => i.Adress))
            {
                UpdateDistributionMovies(selectedMovies, distributionToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateDistributionMovies(selectedMovies, distributionToUpdate);
            PopulateDistributedMovieData(distributionToUpdate);
            return View(distributionToUpdate);
        }
        private void UpdateDistributionMovies(string[] selectedMovies, Distribution distributionToUpdate)
        {
            if (selectedMovies == null)
            {
                distributionToUpdate.DistributedMovies = new List<DistributedMovie>();
                return;
            }
            var selectedMoviesHS = new HashSet<string>(selectedMovies);
            var distributedMovies = new HashSet<int>
            (distributionToUpdate.DistributedMovies.Select(c => c.Movie.ID));
            foreach (var movie in _context.Movies)
            {
                if (selectedMoviesHS.Contains(movie.ID.ToString()))
                {
                    if (!distributedMovies.Contains(movie.ID))
                    {
                        distributionToUpdate.DistributedMovies.Add(new DistributedMovie
                        {
                            DistributionID =
                       distributionToUpdate.ID,
                            MovieID = movie.ID
                        });
                    }
                }
                else
                {
                    if (distributedMovies.Contains(movie.ID))
                    {
                        DistributedMovie movieToRemove = distributionToUpdate.DistributedMovies.FirstOrDefault(i
                       => i.MovieID == movie.ID);
                        _context.Remove(movieToRemove);
                    }
                }
            }
        }

        // GET: Distributions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Distributions == null)
            {
                return NotFound();
            }

            var distribution = await _context.Distributions
                .FirstOrDefaultAsync(m => m.ID == id);
            if (distribution == null)
            {
                return NotFound();
            }

            return View(distribution);
        }

        // POST: Distributions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Distributions == null)
            {
                return Problem("Entity set 'MovieContext.Distributions'  is null.");
            }
            var distribution = await _context.Distributions.FindAsync(id);
            if (distribution != null)
            {
                _context.Distributions.Remove(distribution);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistributionExists(int id)
        {
          return _context.Distributions.Any(e => e.ID == id);
        }
    }
}
