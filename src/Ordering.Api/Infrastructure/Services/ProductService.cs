using Grpc.Net.Client;
using Inventory.Grpc.Api;
using Ordering.Api.Application.Dtos;
using Ordering.Api.Core.Interfaces;

namespace Ordering.Api.Infrastructure.Services;

public class ProductService(IConfiguration configuration) : IProductService
{
    public async Task<ProductDto?> GetValue(string id, CancellationToken cancellationToken = default)
    {
        var section = configuration.GetSection("Grpc");
        using var channel = GrpcChannel.ForAddress(section["ServerBaseAddress"]!);
        var client = new Product.ProductClient(channel);
        var reply = await client.GetValueAsync(new()
        {
            Id = id
        });

        if (reply is null)
        {
            return null;
        }

        return new()
        {
            Name = reply.Name,
            Id = Guid.Parse(reply.Id),
            Quantity = reply.Quantity,
            UnitPrice = (decimal)reply.UnitPrice,
            CreatedAt = DateTime.Parse(reply.CreatedAt),
            UpdatedAt = DateTime.Parse(reply.UpdatedAt)
        };
    }
}