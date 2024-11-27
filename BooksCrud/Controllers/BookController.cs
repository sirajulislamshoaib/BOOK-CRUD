using BooksCrud.Data;
using BooksCrud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace BooksCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly DbService dbService;
        public BookController(DbService dbService)
        {
            this.dbService = dbService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] UpdateBook updateBook, IFormFile? cover, IFormFile? myBook)
        {
            var book = new Book()
            {
                Id=updateBook.Id,
                Title= updateBook.Title,
                Author= updateBook.Author,
                Page= updateBook.Page,
                CategoryId= updateBook.CategoryId,
                PublishDate= updateBook.PublishDate.ToString()
            };
            if (book == null) { 
                return BadRequest(new { Message = "Book data not valid" });
            }
            var coverPath = "";
            var pdfPath = "";
            if (cover != null)
            {
                coverPath = Path.Combine(Directory.GetCurrentDirectory(), "Covers", cover.FileName);
                using (var stream = new FileStream(coverPath, FileMode.Create, FileAccess.Write))
                {
                    await cover.CopyToAsync(stream);
                }
                coverPath = "Covers/" + cover.FileName;
            }

            
            if (myBook != null)
            {
                pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "Books", myBook.FileName);
                using (var stream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
                {
                    await myBook.CopyToAsync(stream);
                }
                pdfPath = "Books/" + myBook.FileName;
            }
            book.PdfBook = pdfPath;
            book.CoverPhoto = coverPath;
            var result = dbService.CreateBook(book);
            if (result)
            {
                return Ok(new {Message="Data saved"});
            }
            return BadRequest(new { Message = "Data not saved." });
        }

        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromForm] UpdateBook updateBook, IFormFile cover=null, IFormFile myBook=null)
        {
            var book = new Book()
            {
                Id= updateBook.Id,
                Title = updateBook.Title,
                Author = updateBook.Author,
                Page = updateBook.Page,
                CategoryId = updateBook.CategoryId,
                PublishDate = updateBook.PublishDate.ToString()
            };
            if (book == null)
            {
                return BadRequest(new { Message = "Book data not valid" });
            }
            var coverPath = "";
            var pdfPath = "";
            if (cover != null)
            {
                coverPath = Path.Combine(Directory.GetCurrentDirectory(), "Covers", cover.FileName);
                using (var stream = new FileStream(coverPath, FileMode.Create, FileAccess.Write))
                {
                    await cover.CopyToAsync(stream);
                }
                coverPath = "Covers/" + cover.FileName;
            }


            if (myBook != null)
            {
                pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "Books", myBook.FileName);
                using (var stream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
                {
                    await myBook.CopyToAsync(stream);
                }
                pdfPath = "Books/" + myBook.FileName;
            }
            book.PdfBook = pdfPath;
            book.CoverPhoto = coverPath;
            var result = dbService.UpdateBook(book);
            if (result)
            {
                return Ok(new { Message = "Data Updated" });
            }
            return BadRequest(new { Message = "Data not updated." });
        }



        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] string Author="", [FromQuery] string Title="", [FromQuery]  int CategoryId=0, [FromQuery] string FromDate="",[FromQuery] string ToDate="")
        {
            var bookFilter = new BookFilter
            {
                Author=Author,
                Title=Title,
                CategoryId=CategoryId,
                FromDate=FromDate,
                ToDate=ToDate
            };
            var result = dbService.BookList(bookFilter);
            return Ok(result);
        }

       
        [HttpGet("SingleUser/{Id:int}")]
        public async Task<IActionResult> SingleUser([FromRoute] int Id)
        {
            try
            {
                if (Id == null || Id == 0)
                {
                    return BadRequest(new { status = false, message = "Id not found" });
                }
                var result = dbService.GetSingleUser(Id);
                return Ok(new { status = true, data = result });

            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = "An internal error occurred" });
            }
        }

        [HttpDelete]
        [Route("Delete/{Id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            try
            {
                if (Id == null || Id == 0)
                {
                    return BadRequest(new {  message = "Id not found" });
                }
                var result = dbService.DeleteBook(Id);
                if (result)
                {
                    return Ok(new { message = "Data Deleted" });
                }
                return BadRequest(new {  message = "Data not Deleted" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Data not Deleted" });
            }
        }
    }
}
