namespace Application.Products.Dtos;

public class UpdateProductDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
