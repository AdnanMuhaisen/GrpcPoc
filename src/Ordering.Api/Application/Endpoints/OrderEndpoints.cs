using Ordering.Api.Application.Commands;
using Ordering.Api.Core.Entities;
using Ordering.Api.Core.Interfaces;
using Ordering.Api.Infrastructure.Data;

namespace Ordering.Api.Application.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/orders");

        group.MapPost("/", async (CreateOrderCommand command,
            OrderingDbContext orderingDbContext,
            IProductService productService,
            CancellationToken cancellationToken) =>
        {
            var product = await productService.GetValue(command.ProductId, cancellationToken);
            if (product is null)
            {
                return Results.Problem(title: "PRODUCT_NOT_FOUND",
                    detail: $"Product with id {command.ProductId} could not be found");
            }

            if (command.Quantity > product.Quantity)
            {
                return Results.Problem(
                    title: "INSUFFICIENT_QUANTITY",
                    detail: $"Product with ID {command.ProductId} does not have enough quantity.");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Quantity = command.Quantity,
                CreatedAt = DateTime.UtcNow,
                CustomerName = command.CustomerName,
                TotalPrice = command.Quantity * product.UnitPrice
            };

            orderingDbContext.Orders.Add(order);
            await orderingDbContext.SaveChangesAsync(cancellationToken);

            //TODO: publish product quantity changed integration event

            return Results.Created();
        });

        return builder;
    }
}