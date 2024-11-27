using BooksCrud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace BooksCrud.Data
{
    public class DbService
    {
        private readonly string _connectionString;
        private readonly string _url;
        private object dbService;

        // Constructor that retrieves connection string from appsettings.json
        public DbService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _url = configuration["URL"];
        }

        public bool CreateBook(Book book)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var sqlQuery = "[USP_INSERT_BOOK] @Title,@Author,@Page,@CategoryId,@CoverPhoto,@PdfBook,@PublishDate";
                    var cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author",book.Author);
                    cmd.Parameters.AddWithValue("@Page", book.Page);
                    cmd.Parameters.AddWithValue("@CategoryId", book.CategoryId);
                    cmd.Parameters.AddWithValue("@CoverPhoto", book.CoverPhoto);
                    cmd.Parameters.AddWithValue("@PdfBook", book.PdfBook);
                    cmd.Parameters.AddWithValue("@PublishDate", book.PublishDate);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
           
        }

        public bool UpdateBook(Book book)
        {
            try
            {
                
                using (var conn = new SqlConnection(_connectionString))
                {
                    // SQL Query to call the stored procedure that updates the book
                    var sqlQuery = "[USP_UPDATE_BOOK] @Id, @Title, @Author, @Page, @CategoryId, @CoverPhoto, @PdfBook, @PublishDate";
                    //var sqlQuery = "UPDATE Book SET Title=@Title,Author=@Author,Page=@Page,CategoryId=@CategoryId,PdfBook=@PdfBook WHERE Id=@Id";

                    // Creating the SQL Command with the stored procedure
                    var cmd = new SqlCommand(sqlQuery, conn);

                    // Adding parameters to the SQL command
                    cmd.Parameters.AddWithValue("@Id", book.Id); 
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);
                    cmd.Parameters.AddWithValue("@Page", book.Page);
                    cmd.Parameters.AddWithValue("@CategoryId", book.CategoryId);
                    cmd.Parameters.AddWithValue("@CoverPhoto", book.CoverPhoto);
                    cmd.Parameters.AddWithValue("@PdfBook", book.PdfBook);
                    cmd.Parameters.AddWithValue("@PublishDate", book.PublishDate);

                    // Open the connection and execute the query
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                // You might want to log the exception here for debugging purposes
                //Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


        public bool DeleteBook(int Id)
        {
            try
            {
                var book = new Book();
                using (var conn = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand("UPDATE Book SET IsActive=@IsActive WHERE Id=@Id", conn);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@IsActive", 0);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                // You might want to log the exception here for debugging purposes
                //Console.WriteLine($"Error: {ex.Message}");
                return false;
            }

        }

        public Book GetSingleUser(int Id)
        {
            var book = new Book();
            using (var conn = new SqlConnection(_connectionString))
            {
                var sqlQuery = "[USP_GET_SINGLE_BOOK] "+Id;
                var cmd = new SqlCommand(sqlQuery, conn);
                conn.Open();
                using (var result = cmd.ExecuteReader())
                {
                    while (result.Read())
                    {
                        book.Id = (int)result["Id"];
                        book.Author = (string)result["Author"];
                        book.Title = (string)result["Title"];
                        book.CategoryId = (int)result["CategoryId"];
                        //book.CategoryName = (string)result["CategoryName"];
                        book.Page = (int)result["Page"];
                        book.CoverPhoto = (string)result["CoverPhoto"] == "" ? "" : _url + (string)result["CoverPhoto"];
                        book.PdfBook = (string)result["PdfBook"] == "" ? "" : _url + (string)result["PdfBook"];
                        book.PublishDate = (string)result["PublishDate"];
                        book.IsActive = (int)result["IsActive"];
                    }
                }
            }
            return book;
        }


        public List<Book> BookList(BookFilter bookFilter)
        {
            var books = new List<Book>();
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("USP_GET_BOOK_LIST @Author,@Title,@FromDate,@ToDate,@CategoryId", conn);
                cmd.Parameters.AddWithValue("@Author", bookFilter.Author);
                cmd.Parameters.AddWithValue("@Title", bookFilter.Title);
                cmd.Parameters.AddWithValue("@FromDate", bookFilter.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", bookFilter.ToDate);
                cmd.Parameters.AddWithValue("@CategoryId", bookFilter.CategoryId);
                conn.Open();
                using (var result = cmd.ExecuteReader())
                {
                    while (result.Read())
                    {
                        books.Add(new Book
                        {
                            Id = (int)result["Id"],
                            Author = (string)result["Author"],
                            Title = (string)result["Title"],
                            CategoryId = (int)result["CategoryId"],
                            CategoryName = (string)result["CategoryName"],
                            Page = (int)result["Page"],
                            CoverPhoto = (string)result["CoverPhoto"] == "" ? "" : _url + (string)result["CoverPhoto"],
                            PdfBook = (string)result["PdfBook"] == "" ? "" : _url + (string)result["PdfBook"],
                            PublishDate = (string)result["PublishDate"],
                            IsActive = (int)result["IsActive"]
                        });
                    }
                }
            }
            return books;
        }
    }
}
