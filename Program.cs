using Microsoft.EntityFrameworkCore;
using Project4_1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Teachers database
builder.Services.AddDbContext<TeacherDbContext>(options => 
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Passward database
builder.Services.AddDbContext<PasswordDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("PasswordHashConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
