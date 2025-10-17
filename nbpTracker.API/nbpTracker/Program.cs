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
builder.Services.AddScoped<ICurrencyRatesFetcher, CurrencyRatesFetcher>();
builder.Services.AddScoped<ICurrencyRatesService, CurrencyRatesService>();
builder.Services.AddHttpClient<CurrencyRatesFetcher>();

builder.Services.AddHostedService<CurrencySyncWorker>();

builder.Services.AddAutoMapper(cfg => { }, typeof(MapperProfile));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=currencyrates.db"));

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
