using FlightFinderApi.Data;
using FlightFinderApi.Services.Implementations;
using FlightFinderApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//From .AddJsonOptions I add for token
builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Add dependency Injections for the following services 
builder.Services.AddScoped<IRootService, RootService>();
builder.Services.AddScoped<IItineraryService, ItineraryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add dependency injection to connect with Sql Server
builder.Services.AddDbContext<FlightFinderDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("FlightFinderApiConnectionString")));





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// I add for Token after .AddSwaggerGen
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FlightFinderApi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
        }
    },
      new string[]{}
    }

    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))

    };
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    //I add for token from (c => c.SwaggerEndpoint)
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoJWTToken v1"));
}

// added folowing line to connect with react
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));


app.UseHttpsRedirection();

// Add for authentication
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();
