using System.ComponentModel.DataAnnotations.Schema;

namespace BooksCrud.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Page { get; set; }
        public int CategoryId { get; set; }
        public string CoverPhoto { get; set; }
        public string PdfBook { get; set; }
        public string PublishDate { get; set; }
        [NotMapped]
        public int IsActive { get; set; } = 1;
        [NotMapped]
        public string CategoryName { get; set; }
    }
}
