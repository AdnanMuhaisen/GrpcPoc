namespace Ordering.Api.Core.Entities;

public class Order
{
    public Guid Id { get; set; }

    public string CustomerName { get; set; } = null!;

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }
}