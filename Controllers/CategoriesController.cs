using ITExpertTEST.Data;
using ITExpertTEST.Models;
using ITExpertTEST.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITExpertTEST.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDBContext _context;

        public CategoriesController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _context.Categories
                .Include(c => c.FilmCategories)
                .ToListAsync();

                var categoryDtos = categories.Select(c => new CategoryViewModel
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    ParentCategoryId = c.ParentCategoryId,
                    FilmCount = c.FilmCategories.Count,
                    NestingLevel = GetNestingLevel(c)
                }).ToList();
                ViewData["Categories"] = categories;
                return View(categoryDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while updating the film." });
            }
        }

        [HttpPost]
        public ActionResult AddCategory(string categoryName, int? parentCategoryId)
        {
            try
            {
                var newCategory = new Category
                {
                    Name = categoryName,
                    ParentCategoryId = parentCategoryId
                };

                _context.Categories.Add(newCategory);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(int categoryId, string categoryName, int? parentCategoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            if (category == null)
                return NotFound();

            try
            {
                if (parentCategoryId.HasValue && parentCategoryId == 0)
                    return Json(new { success = false, error = "Invalid parent category." });
                
                if (parentCategoryId.HasValue && IsCircularReference(categoryId, parentCategoryId.Value))
                    return Json(new { success = false, error = "Adding this category would create a circular reference." });
                
                category.Name = categoryName;
                category.ParentCategoryId = parentCategoryId;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);

                if (category == null)
                    return NotFound();

                int categoryIdToDelete = category.CategoryId;

                var childCategories = await _context.Categories
                    .Where(c => c.ParentCategoryId == categoryIdToDelete)
                    .ToListAsync();

                foreach (var childCategory in childCategories)
                {
                    childCategory.ParentCategoryId = null;
                }

                _context.Categories.Remove(category);

                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


        private int GetNestingLevel(Category category)
        {
            int level = 0;
            var parentCategory = category.ParentCategory;

            while (parentCategory != null)
            {
                level++;
                parentCategory = parentCategory.ParentCategory;
            }

            return level;
        }

        private bool IsCircularReference(int categoryIdToCheck, int? parentCategoryId)
        {
            if (!parentCategoryId.HasValue)
                return false;

            if (parentCategoryId == categoryIdToCheck)
                return true;

            var parentCategory = _context.Categories.Find(parentCategoryId);
            if (parentCategory == null)
                return false;

            return IsCircularReference(categoryIdToCheck, parentCategory.ParentCategoryId);
        }
    }
}
