using BlazorBlog.API.Models;
using BlazorBlog.API.Services.Filters;
using BlazorBlog.API.Validators.Article;
using BlazorBlog.API.Validators.Email;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var emailConfig = builder.Configuration.GetSection("EmailConfiguration")
//        .Get<EmailConfiguration>();
//builder.Services.AddSingleton(emailConfig);

builder.Services.AddControllers(options =>
{
	options.Filters.Add<ValidationFilter>();
})
	.AddFluentValidation(config =>
	{
		config.RegisterValidatorsFromAssemblyContaining<ContactEmailValidator>();
		config.DisableDataAnnotationsValidation = true;

    })
	.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
	.AddNewtonsoftJson(options =>
	{
		options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

    });


builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		//policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); //Siteye her giren kullan?c? istekleri g?rebilir demektir.
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

//var staticFileOptions = new StaticFileOptions
//{
//    OnPrepareResponse = (context) =>
//    {
//        var fn = context.File.Name.ToLowerInvariant();
//        if (fn.EndsWith(".pdf"))
//        {
//        }
//        else
//        {
//            context.Context.Response.Headers.Add("Cache-Control", "public, max-age=15552000"); // 180 days
//        }
//    }
//};


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
