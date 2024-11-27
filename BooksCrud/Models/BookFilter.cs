using Microsoft.AspNetCore.Mvc;

namespace BooksCrud.Models
{
    public class BookFilter
    {
        public string Author { get; set; } = "";
        public string Title { get; set; } = "";
        public int CategoryId { get; set; } = 0;
        public string FromDate { get; set; } = "";
        public string ToDate { get; set; } = "";
    }
}
