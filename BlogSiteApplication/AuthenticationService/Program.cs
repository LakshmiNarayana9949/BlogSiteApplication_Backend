using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Design;
using AuthenticationService.Services;
using RegistrationService.Services;
using RegistrationService.DBContext;
using AuthenticationService.DBContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IJWTTokenInterface, JWTTokenImpl>();
//builder.Services.AddScoped<IUserInterface, UserImpl>();

builder.Services.AddDbContext<AuthenticationDBContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("BlogSiteDB")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

app.UseDeveloperExceptionPage();
