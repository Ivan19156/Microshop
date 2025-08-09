
namespace OrchestratorService.Dtos;

public class ProductDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public Guid Id { get; set; }
}


