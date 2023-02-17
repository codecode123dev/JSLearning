using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBOOK.Data;
using FPTBOOK.Models;
using Newtonsoft.Json;

namespace FPTBOOK.Controllers
{
    public class BooksController : Controller
    {
        

        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        


        //search
        public ActionResult Index(string searchString)
        {
            var _book = IndexCart();


            var books = from l in _context.Book 
                        select l;

            if (!String.IsNullOrEmpty(searchString)) 
            {
                books = books.Where(s => s.Name.Contains(searchString)); 
            }

            return View(books); 


        }



        
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Date,Price,ImageUrl,CategoryId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id", book.CategoryId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id", book.CategoryId);
            return View(book);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Date,Price,ImageUrl,CategoryId")] Book book)
        {
            if (id != book.Id)
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
                    if (!BookExists(book.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Book == null)
            {
                return Problem("Entity set 'BookASMContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }

      



        public ActionResult IndexCart()
        {
            var _book = getAllBook();
            ViewBag.book = _book;
            return View();
        }

        public List<Book> getAllBook()
        {
            return _context.Book.ToList();
        }

        public Book getDetailBook(int id)
        {
            var book = _context.Book.Find(id);
            return book;
        }

        public IActionResult addCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart == null)
            {
                var book = getDetailBook(id);
                List<Cart> listCart = new List<Cart>()
                {
                    new Cart
                    {
                        Book = book,
                        Quantity = 1
                    }
                };
                //HttpContext.Session.SetString("cart", JsonConvert.S)
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(listCart));

            }
            else
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                bool check = true;
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].Book.Id == id)
                    {
                        dataCart[i].Quantity++;
                        check = false;
                    }
                }
                if (check)
                {
                    dataCart.Add(new Cart
                    {
                        Book = getDetailBook(id),
                        Quantity = 1
                    });

                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
            }
            //return RedirectToAction(nameof(IndexCart));
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ListCart()
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                if (dataCart.Count > 0)
                {
                    ///????????? "carts" or Cart
                    ViewBag.carts = dataCart;
                    return View();
                }
            }
            //return RedirectToAction(nameof(IndexCart));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]

        public IActionResult updateCart(int id, int quantity)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                if (quantity > 0)
                {
                    for (int i = 0; i < dataCart.Count; i++)
                    {
                        if (dataCart[i].Book.Id == id)
                        {
                            dataCart[i].Quantity = quantity;
                        }
                    }
                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                }
                var cart2 = HttpContext.Session.GetString("cart");
                return Ok(quantity);
            }
            return BadRequest();
        }

        public IActionResult deleteCart(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
                for (int i = 0; i < dataCart.Count; i++)
                {
                    if (dataCart[i].Book.Id == id)
                    {
                        dataCart.RemoveAt(i);
                    }
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
                return RedirectToAction(nameof(ListCart));
            }
            //return RedirectToAction(nameof(IndexCart));
            return RedirectToAction(nameof(Index));
        }
    }
}
