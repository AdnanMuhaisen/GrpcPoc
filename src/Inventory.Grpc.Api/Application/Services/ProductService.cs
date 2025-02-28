using Grpc.Core;
using Inventory.Grpc.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Inventory.Grpc.Api.Application.Services;

public class ProductService(AppDbContext appDbContext) : Product.ProductBase
{
    public override async Task<CreateProductResponse> Create(CreateProductRequest request, ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.UnitPrice <= 0 || request.Quantity <= 0)
        {
            ThrowValidationError();
        }

        if (await appDbContext.Products.AnyAsync(x => x.Name == request.Name))
        {
            ThrowValidationError();
        }

        var product = new Core.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Quantity = request.Quantity,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UnitPrice = (decimal)request.UnitPrice
        };

        appDbContext.Products.Add(product);
        await appDbContext.SaveChangesAsync();

        var response = new CreateProductResponse
        {
            Name = product.Name,
            Id = product.Id.ToString(),
            Quantity = product.Quantity,
            UnitPrice = (double)product.UnitPrice,
            CreatedAt = product.CreatedAt.ToString(CultureInfo.CurrentCulture),
            UpdatedAt = product.UpdatedAt.ToString(CultureInfo.CurrentCulture)
        };

        return response;
    }

    public override async Task<UpdateProductResponse> Update(UpdateProductRequest request, ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Id)
            || !Guid.TryParse(request.Id, out _)
            || string.IsNullOrWhiteSpace(request.Name)
            || request.UnitPrice <= 0
            || request.Quantity <= 0)
        {
            ThrowValidationError();
        }

        var id = Guid.Parse(request.Id);
        var product = await appDbContext.Products.SingleOrDefaultAsync(x => x.Id == id);
        if (product is null)
        {
            ThrowProductDoesNotExist();
        }

        if (await appDbContext.Products.AnyAsync(x => x.Name == request.Name))
        {
            ThrowValidationError();
        }

        product.Name = request.Name;
        product.Quantity = request.Quantity;
        product.UnitPrice = (decimal)request.UnitPrice;

        appDbContext.Products.Update(product);
        await appDbContext.SaveChangesAsync();

        return new UpdateProductResponse
        {
            Success = true
        };
    }

    public override async Task<GetAllProductsResponse> Get(GetAllProductsRequest request, ServerCallContext context)
    {
        var products = await appDbContext
            .Products
            .AsNoTracking()
            .ToListAsync();

        var response = new GetAllProductsResponse();
        response.Products.AddRange(products.Select(x => new GetProductResponse
        {
            Name = x.Name,
            Id = x.Id.ToString(),
            Quantity = x.Quantity,
            UnitPrice = (double)x.UnitPrice,
            CreatedAt = x.CreatedAt.ToString(CultureInfo.CurrentCulture),
            UpdatedAt = x.UpdatedAt.ToString(CultureInfo.CurrentCulture)
        }));

        return response;
    }

    public override async Task<GetProductResponse> GetValue(GetProductRequest request, ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Id) || !Guid.TryParse(request.Id, out _))
        {
            ThrowValidationError();
        }

        var id = Guid.Parse(request.Id);
        var product = await appDbContext
            .Products
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id);

        if (product is null)
        {
            ThrowProductDoesNotExist();
        }

        return new GetProductResponse
        {
            Name = product.Name,
            Id = product.Id.ToString(),
            Quantity = product.Quantity,
            UnitPrice = (double)product.UnitPrice,
            CreatedAt = product.CreatedAt.ToString(CultureInfo.CurrentCulture),
            UpdatedAt = product.UpdatedAt.ToString(CultureInfo.CurrentCulture)
        };
    }

    public override async Task<DeleteProductResponse> Delete(DeleteProductRequest request, ServerCallContext context)
    {
        if (string.IsNullOrWhiteSpace(request.Id) || !Guid.TryParse(request.Id, out _))
        {
            ThrowValidationError();
        }

        var id = Guid.Parse(request.Id);
        var product = await appDbContext
            .Products
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id);

        if (product is null)
        {
            ThrowProductDoesNotExist();
        }

        appDbContext.Products.Remove(product);
        await appDbContext.SaveChangesAsync();

        return new DeleteProductResponse
        {
            Success = true
        };
    }

    [DoesNotReturn]
    private static void ThrowValidationError() =>
        throw new RpcException(new Status(StatusCode.InvalidArgument, $"One or more validation error occurs"));

    [DoesNotReturn]
    private static void ThrowProductDoesNotExist() =>
        throw new RpcException(new Status(StatusCode.NotFound, "Product could not be found"));
}