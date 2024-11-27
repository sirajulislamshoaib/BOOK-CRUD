using BooksCrud.Data;
using Microsoft.Extensions.FileProviders;
using System.IO;
var coversPath = Path.Combine(Directory.GetCurrentDirectory(), "Covers");
var booksPath = Path.Combine(Directory.GetCurrentDirectory(), "Books");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<DbService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowAllOrigins");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(); 
//add  static file path here
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(coversPath),
    RequestPath = "/Covers"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(booksPath),
    RequestPath = "/Books"
});
app.UseAuthorization();

app.MapControllers();

app.Run();
