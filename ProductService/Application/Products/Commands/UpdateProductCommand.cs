using Application.Products.Dtos;
using MediatR;

namespace Application.Products.Commands;

public class UpdateProductCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public UpdateProductDto Product { get; set; }

    public UpdateProductCommand(int id, UpdateProductDto product)
    {
        Id = id;
        Product = product;
    }
}
