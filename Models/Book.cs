namespace FPTBOOK.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<Order>? Order { get; set; }// mot book co nhieu order
    }
}
