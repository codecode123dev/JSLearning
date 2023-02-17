namespace FPTBOOK.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Book>? Book { get; set; }//mootj cate co nhieu sach
    }
}
