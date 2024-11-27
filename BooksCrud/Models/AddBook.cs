using System.ComponentModel.DataAnnotations.Schema;

namespace BooksCrud.Models
{
    public class AddBook
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Page { get; set; }
        public int CategoryId { get; set; }
        public DateOnly PublishDate { get; set; }
    }
}
