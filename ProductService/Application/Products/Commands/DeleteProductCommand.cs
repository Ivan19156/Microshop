using MediatR;

namespace Application.Products.Commands;

public class DeleteProductCommand : IRequest<Unit>
{
    public int Id { get; }

    public DeleteProductCommand(int id)
    {
        Id = id;
    }
}
