using FPTBOOK.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FPTBOOK.Data;
using FPTBOOK.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FPTBOOK.Controllers
{
    public class HomeController : Controller
    {
        

        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            dbContext = context;
            this.webHostEnvironment = webHostEnvironment;
        }

      


        //search
        public ActionResult Index(string searchString)
        {
            var books = from l in dbContext.Book // lấy toàn bộ liên kết
                        select l;

            if (!String.IsNullOrEmpty(searchString)) // kiểm tra chuỗi tìm kiếm có rỗng/null hay không
            {
                books = books.Where(s => s.Name.Contains(searchString)); //lọc theo chuỗi tìm kiếm
            }

            return View(books); //trả về kết quả
        }

        public IActionResult New()
        {
            return View();
        }
       

        
    }
}