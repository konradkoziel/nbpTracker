using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using nbpTracker;
using nbpTracker.Common;
using nbpTracker.Services;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICurrencyRatesService, CurrencyRatesService>();
builder.Services.AddHttpClient<ICurrencyRatesFetcher, CurrencyRatesFetcher>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddHostedService<CurrencySyncWorker>();

builder.Services.AddAutoMapper(cfg => { }, typeof(MapperProfile));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=currencyrates.db"));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowLocalhost");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
