using Microsoft.EntityFrameworkCore;
using SoundApplication.Services.Abstract;
using SoundApplication.Services.Concrete;
using SoundApplication.Services.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("https://*:10061");
builder.Services.AddScoped<ISoundService, SoundService>();

var connection = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<SoundDbContext>(a => a.UseSqlServer(connection));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
