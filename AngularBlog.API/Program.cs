using BlazorBlog.API.Models;
using BlazorBlog.API.Services.Filters;
using BlazorBlog.API.Validators.Email;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var emailConfig = builder.Configuration.GetSection("EmailConfiguration")
//        .Get<EmailConfiguration>();
//builder.Services.AddSingleton(emailConfig);

builder.Services.AddControllers(options  =>options.Filters.Add<ValidationFilter>())
	.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<ContactEmailValidator>())
	.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
	.AddNewtonsoftJson(options =>
	{
		options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

    });


builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		//policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); //Siteye her giren kullanýcý istekleri görebilir demektir.
		policy.WithOrigins("http://localhost:4200", "https://localhost:7166", "http://localhost:5037").AllowAnyHeader().AllowAnyMethod();
	});
});


builder.Services.AddDbContext<BlazorBlogDBContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("Onur"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
