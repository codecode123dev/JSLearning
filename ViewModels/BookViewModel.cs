namespace FPTBOOK.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public IFormFile ImageUrl { get; set; }
    }
}
