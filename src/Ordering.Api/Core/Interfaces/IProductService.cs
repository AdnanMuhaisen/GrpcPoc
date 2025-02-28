using Ordering.Api.Application.Dtos;

namespace Ordering.Api.Core.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetValue(string id, CancellationToken cancellationToken = default);
}