using Microsoft.EntityFrameworkCore;
using MyDemoApi.DataBase;
using MyDemoApi.EndPoints;
using MyDemoApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// var connectionString = builder.Configuration.GetConnectionString("ConStr");
// var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
// connectionString = string.Format(connectionString, password);

// var appConfigSetting = builder.Configuration.GetSection("AppConfigSetting");

// var dataSource = Environment.GetEnvironmentVariable("dataSource") ?? appConfigSetting.GetValue<string>("dataSource");
// var userId = Environment.GetEnvironmentVariable("userId") ?? appConfigSetting.GetValue<string>("userId");
// var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD") ?? appConfigSetting.GetValue<string>("password");

//var placeHolder = builder.Configuration.GetConnectionString("ConStr");//?? "Data Source={0};Initial Catalog=ecomDB;User Id={1}; Password={2};trusted_Connection=True;TrustServerCertificate=true";


//var connectionString = Environment.GetEnvironmentVariable("ConStr") ?? builder.Configuration.GetConnectionString("ConStr");  //string.Format(placeHolder, dataSource, userId, password);

//Environment.GetEnvironmentVariable("ConStr") ?? builder.Configuration.GetConnectionString("ConStr");



builder.Services.AddDbContext<myDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")
         // ?? throw new InvalidOperationException("Connection string 'ProductDbContext' not found.")
         )
    );

/*
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = redisConnection;
});
*/
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async httpContext =>
    {
        var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
        if (pds == null
            || !await pds.TryWriteAsync(new() { HttpContext = httpContext }))
        {
            // Fallback behavior


            await httpContext.Response.WriteAsync("Fallback: An error occurred.");
        }
    });
});


System.Diagnostics.Debug.WriteLine("============================");
System.Diagnostics.Debug.WriteLine(app.Environment.EnvironmentName);
System.Diagnostics.Debug.WriteLine("============================");


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//app.ApplyMigrations();
//}

app.UseHttpsRedirection();


app.MapProductEndpoints();

app.Run();
