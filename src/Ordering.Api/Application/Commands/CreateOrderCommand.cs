namespace Ordering.Api.Application.Commands;

public class CreateOrderCommand
{
    public string CustomerName { get; set; } = null!;

    public string ProductId { get; set; }

    public int Quantity { get; set; }
}