using Microsoft.EntityFrameworkCore;
using MyDemoApi.DataBase;
using MyDemoApi.EndPoints;
using MyDemoApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// var connectionString = builder.Configuration.GetConnectionString("ConStr");
// var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
// connectionString = string.Format(connectionString, password);

var connectionString = Environment.GetEnvironmentVariable("ConStr") ?? builder.Configuration.GetConnectionString("ConStr");

builder.Services.AddDbContext<myDBContext>(options =>
    options.UseSqlServer(connectionString
            ?? throw new InvalidOperationException("Connection string 'ProductDbContext' not found.")
         )
    );

var redisConnection = Environment.GetEnvironmentVariable("RedisConnection") ?? builder.Configuration.GetConnectionString("RedisConnection");

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = redisConnection;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();


app.MapProductEndpoints();

app.Run();
