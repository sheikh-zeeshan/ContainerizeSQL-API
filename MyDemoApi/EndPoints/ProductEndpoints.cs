using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MyDemoApi.Contracts;
using MyDemoApi.DataBase;
using MyDemoApi.Entities;
using MyDemoApi.Extensions;

namespace MyDemoApi.EndPoints;


public static class ProductEndpoints
{
    static bool cacheServerIsAccessiable = false;

    public static void MapProductEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {

        var group = endpointRouteBuilder
                   .MapGroup("/api/Product")
                   .WithTags(nameof(Product));

        group.MapPost("/", async (
            CreateProductRequest request,
            myDBContext context,
            CancellationToken ct) =>
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };

            context.Add(product);

            await context.SaveChangesAsync(ct);

            return Results.Ok(product);
        })
        .WithName("AddProduct")
        .WithOpenApi();

        group.MapGet("/", async Task<Results<Ok<List<Product>>, NoContent>> (myDBContext db, IDistributedCache cache) =>
                {
                    List<Product> products = null;

                    var cacheKey = $"allproducts";
                    try
                    {
                        var Cachedproducts = await cache.GetStringAsync(cacheKey);
                        if (Cachedproducts is not null)
                        {
                            products = JsonSerializer.Deserialize<List<Product>>(Cachedproducts);
                            cacheServerIsAccessiable = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        //do something more useful
                        products = null;
                        cacheServerIsAccessiable = false;
                    }

                    if (products is null)
                    {
                        products = await db.Products.ToListAsync();
                        if (cacheServerIsAccessiable)
                            await cache.SetStringAsync(cacheKey,
                                         JsonSerializer.Serialize(products), CacheOptions.DefaultExpiration);
                    }

                    return products is null ? TypedResults.NoContent() : TypedResults.Ok(products);
                    // return TypedResults.Ok(await db.Products.ToListAsync());
                })
                .WithName("GetAllProducts")
                .WithOpenApi();
    }

}