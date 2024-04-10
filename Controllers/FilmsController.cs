using ITExpertTEST.Data;
using ITExpertTEST.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITExpertTEST.Controllers
{
    public class FilmsController : Controller
    {
        private readonly AppDBContext _context;

        public FilmsController(AppDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(string categoryFilter = null)
        {
            try
            {
                IQueryable<Film> filmsQuery = _context.Films
                .Include(f => f.FilmCategories)
                .ThenInclude(fc => fc.Category);

                if (!string.IsNullOrEmpty(categoryFilter))
                {
                    int categoryId = int.Parse(categoryFilter);
                    filmsQuery = filmsQuery.Where(f => f.FilmCategories.Any(fc => fc.CategoryId == categoryId));
                }

                var films = filmsQuery.ToList();

                var categories = _context.Categories.ToList();

                ViewData["Categories"] = categories;
                ViewData["SelectedCategory"] = categoryFilter;

                return View(films);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the film." });
            }
        }

        [HttpPost]
        public ActionResult AddFilm(string filmName, string filmDirector, DateTime releaseDate)
        {
            try
            {
                var newFilm = new Film
                {
                    FilmName = filmName,
                    FilmDirector = filmDirector,
                    Release = releaseDate
                };

                _context.Films.Add(newFilm);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the film." });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var film = await _context.Films
                .Include(f => f.FilmCategories)
                    .ThenInclude(fc => fc.Category)
                .FirstOrDefaultAsync(f => f.FilmId == id);

                if (film == null)
                    return NotFound();

                var allCategories = _context.Categories.ToList();
                ViewData["AllCategories"] = allCategories;

                return View(film);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the film." });
            }
        }


        [HttpPost]
        public IActionResult UpdateFilmCategories(int filmId, int[] categoryIds)
        {
            try
            {
                var film = _context.Films
                .Include(f => f.FilmCategories)
                .FirstOrDefault(f => f.FilmId == filmId);

                if (film == null)
                {
                    return NotFound();
                }

                film.FilmCategories.Clear();

                foreach (var categoryId in categoryIds)
                {
                    film.FilmCategories.Add(new FilmCategory { FilmId = filmId, CategoryId = categoryId });
                }

                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the film." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFilm(int filmId, string filmName, string filmDirector, DateTime releaseDate)
        {
            var film = await _context.Films.FindAsync(filmId);

            if (film == null)
            {
                return NotFound();
            }

            try
            {
                film.FilmName = filmName;
                film.FilmDirector = filmDirector;
                film.Release = releaseDate;

                _context.Films.Update(film);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the film." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFilm(int filmId)
        {
            var film = await _context.Films.FindAsync(filmId);

            if (film == null)
                return NotFound();

            try
            {
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "An error occurred while deleting the film." });
            }
        }
    }
}
