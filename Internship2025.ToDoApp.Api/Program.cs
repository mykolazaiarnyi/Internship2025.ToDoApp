using FluentValidation;
using FluentValidation.AspNetCore;
using Internship2025.ToDoApp.Api.Middlewares;
using Internship2025.ToDoApp.Api.Validators;
using Internship2025.ToDoApp.Data;
using Internship2025.ToDoApp.Domain.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = builder.Configuration.GetConnectionString("ToDoAppDb")!;
builder.Services.AddDbContext<ToDoAppDbContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddScoped<ToDoItemsService>();
builder.Services.AddScoped<ICurrentUserService, MockCurrentUserService>();

builder.Services.AddValidatorsFromAssemblyContaining<UpdateToDoItemDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
