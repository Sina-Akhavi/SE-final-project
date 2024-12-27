using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Paradaim.Gateway.GYM.Interfaces;
using Paradaim.Gateway.GYM;
using Paradaim.Gateway.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // Make sure this is included

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Paradaim API",
        Version = "v1",
        Description = "Api for Paradaim",
    });
});
builder.Services.AddDbContext<ParadaimDbContext>(options =>
                        options.UseMySql(
                            "Server=116.203.210.161;Database=ParadaimDB;User=Paradaim;Password=I&Youmm68", 
                            new MySqlServerVersion(new Version(8, 0, 31))
                        ));

// Add CORS policy to allow requests from the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Replace with your frontend's URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register your gateways
builder.Services.AddScoped<IFaqGateway, FaqGateway>();
builder.Services.AddScoped<IGymProgramGateway, GymProgramGateway>();
builder.Services.AddScoped<IIconGateway, IconGateway>();
builder.Services.AddScoped<ILinkGateway, LinkGateway>();
builder.Services.AddScoped<IPlanGateway, PlanGateway>();
builder.Services.AddScoped<ITestimonialGateway, TestimonialGateway>();
builder.Services.AddScoped<IValueGateway, ValueGateway>();
builder.Services.AddScoped<ITrainerGateway, TrainerGateway>();
builder.Services.AddScoped<ISocialGateway, SocialGateway>();
builder.Services.AddScoped<IUserGateway, UserGateway>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply the CORS policy
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
