using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookApp.DbContextManagar;
using BookApp.Models;
using BookApp.Helper;
using BookApp.ViewModel;

namespace BookApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Books
        public IActionResult Index()
        {
            List<Book> booksObj = _context.books.ToList();
            return View(booksObj);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            // Fetch categories from the database
            var categories = _context.categorys.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            // Check if categories are available
            if (categories == null || !categories.Any())
            {
                ModelState.AddModelError(string.Empty, "No categories available. Please add a category first.");
                return View();
            }

            // Pass categories to the view
            ViewBag.CategoryId = new SelectList(categories, "Value", "Text");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Save the image to wwwroot/Images
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(model.Image.FileName);
                string extension = Path.GetExtension(model.Image.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fileStream);
                }

                // Save the URL to the database
                string imageUrl = "/Images/" + fileName;

                var book = new Book
                {
                    Name = model.Name,
                    Description = model.Description,
                    Auther = model.Auther,
                    Date = DateOnly.FromDateTime(model.Date),
                    Rate = model.Rate,
                    ImageURL = imageUrl,
                    CategoryId = model.CategoryId
                };

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.categorys, "Id", "Name", model.CategoryId);
            return View(model);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.categorys, "Id", "Name", book.CategoryId);
            return View(book);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            ViewData["CategoryId"] = new SelectList(_context.categorys, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


    // POST: Books/Delete/5
    [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.books.FindAsync(id);
            if (book != null)
            {
                _context.books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/ReadLater
        public IActionResult ReadLater()
        {
            var booksInReadLater = _context.toRead
                .Include(r => r.Books)
                .Select(r => r.Books)
                .ToList();
            return View(booksInReadLater);
        }

        // POST: Books/AddToReadLater/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToReadLater(int id)
        {
            var readLater = new ToRead { BookId = id,State="To Read" };
            _context.toRead.Add(readLater);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public IActionResult AddToReadLater()
        //{
        //    return RedirectToAction(nameof(ReadLater));
        //}


        private bool BookExists(int id)
        {
            return _context.books.Any(e => e.BookId == id);
        }


    }
}
