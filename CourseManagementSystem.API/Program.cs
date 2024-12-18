using CourseManagementSystem.API.Services.Interfaces;
using CourseManagementSystem.API.Services;
using CourseManagementSystem.API.Repositories.Interfaces;
using CourseManagementSystem.API.Repositories;
using CourseManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using CourseManagementSystem.API.Factories.Interfaces;
using CourseManagementSystem.API.Factories;
using OnlineCourseManagementSystem.API.Factories;
using CourseManagementSystem.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

// Register local services
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

// Register repositories
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

// Register factories
builder.Services.AddScoped<ICourseFactory, CourseFactory>();

var app = builder.Build();

app.UseMiddleware<CustomExceptionMiddleware>();
app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext context) =>
{
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    context.Response.ContentType = "application/json";
    var response = new { message = "An unexpected error occurred." };
    return context.Response.WriteAsJsonAsync(response);
});

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
